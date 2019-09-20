// This file is a port of the very fine Tigra Astronomy driver for the
// Adafruit V2 motor shields from Microsoft .NET Micro Framework to
// nanoFramework and the AbstractIO project. See http://tigra-astronomy.com/, especially
// http://tigra-astronomy.com/stepper-motor-control-for-net-microframework and
// https://bitbucket.org/tigra-astronomy/ta.netmf.motorcontrol. Thank you
// very much, dear Tigra Astronomy team, for the fine work you have done and
// for using the MIT license, so this port was possible.
// Here is the original TA copyright notice:

// This file is part of the TA.NetMF.MotorControl project
// 
// Copyright © 2014-2014 Tigra Astronomy, all rights reserved.
// This source code is licensed under the MIT License, see http://opensource.org/licenses/MIT
// 
// File: StepperMotor.cs  Created: 2014-10-14@03:53
// Last modified: 2014-11-30@13:57 by Tim

using System;
using System.Threading;

namespace AbstractIO
{
    /// <summary>
    /// A two-phase microstepping motor. Provides an implementation of <see cref="IStepDrive"/> that can perform
    /// stepsPerStepCycle of variable resolution, half steps or full steps.
    /// </summary>
    public class StepperMotor : IStepDrive
    {
        readonly int _maxIndex;
        readonly ISingleOutput _phase1Output;
        readonly ISingleOutput _phase2Output;
        float[] _inPhaseDutyCycle;
        float[] _outOfPhaseDutyCycle;
        int _phaseIndex;
        BooleanSettableInput _applyOutputPower = new BooleanSettableInput(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="StepperMotor"/> class.
        /// </summary>
        /// <param name="phase1Output">The H-Bridge that controls motor phase 1.</param>
        /// <param name="phase2Output">The H-Bridge that controls motor phase 2.</param>
        /// <param name="stepsPerStepCycle">The steps per step cycle.</param>
        public StepperMotor(
            ISingleOutput phase1Output,
            ISingleOutput phase2Output,
            int stepsPerStepCycle)
        {
            if (phase1Output == null) throw new ArgumentNullException(nameof(phase1Output));
            if (phase2Output == null) throw new ArgumentNullException(nameof(phase2Output));
            if (phase1Output == phase2Output) throw new ArgumentException("Use different phaseOutputs.");

            _phase1Output = phase1Output;
            _phase2Output = phase2Output;
            _maxIndex = stepsPerStepCycle - 1;
            _phaseIndex = 0;

            // Configure step tables:
            if (stepsPerStepCycle == 4)
            {
                ComputeWholeStepTables();
            }
            else if (stepsPerStepCycle == 8)
            {
                ComputeHalfStepTables();
            }
            else if (stepsPerStepCycle >= 8)
            {
                ComputeMicrostepTables(stepsPerStepCycle);
            }
            else
            {
                throw new ArgumentException(
                    "Use 4 for full steps; 8 for half steps; or >8 for stepsPerStepCycle", nameof(stepsPerStepCycle));
            }
        }

        void ComputeHalfStepTables()
        {
            _inPhaseDutyCycle =
                new[] { +1.0f, +1.0f, +0.0f, -1.0f, -1.0f, -1.0f, +0.0f, +1.0f };

            _outOfPhaseDutyCycle =
                new[] { +0.0f, +1.0f, +1.0f, +1.0f, +0.0f, -1.0f, -1.0f, -1.0f };
        }

        void ComputeWholeStepTables()
        {
            _inPhaseDutyCycle =
                new[] { +1.0f, -1.0f, -1.0f, +1.0f };

            _outOfPhaseDutyCycle =
                new[] { +1.0f, +1.0f, -1.0f, -1.0f };
        }

        public void ReleaseHoldingTorque()
        {
            _phase1Output.Value = 0.0f;
            _phase2Output.Value = 0.0f;
        }

        void ComputeMicrostepTables(int microsteps)
        {
            // This implementation prefers performance over memory footprint.
            var radiansPerIndex = (2 * (float)System.Math.PI) / (microsteps - 1);
            _inPhaseDutyCycle = new float[microsteps];
            _outOfPhaseDutyCycle = new float[microsteps];
            for (var i = 0; i < microsteps; ++i)
            {
                var phaseAngle = i * radiansPerIndex;
                _inPhaseDutyCycle[i] = System.Math.Sin(phaseAngle);
                _outOfPhaseDutyCycle[i] = System.Math.Cos(phaseAngle);
            }
        }

        public void MoveSteps(int steps, int pauseInMs)
        {
            int amount = Math.Abs(steps);
            int step = (steps < 0) ? -1 : +1;
            for (int i = 0; i < amount; i++)
            {
                _phaseIndex += step;
                if (_phaseIndex > _maxIndex)
                {
                    _phaseIndex = 0;
                }
                else if (_phaseIndex < 0)
                {
                    _phaseIndex = _maxIndex;
                }
                _phase1Output.Value = _inPhaseDutyCycle[_phaseIndex];
                _phase2Output.Value = _outOfPhaseDutyCycle[_phaseIndex];
                if (pauseInMs > 0)
                {
                    Thread.Sleep(pauseInMs);
                }
            }
        }
    }
}
