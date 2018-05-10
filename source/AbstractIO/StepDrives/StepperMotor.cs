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
        readonly IDoubleOutput _phase1Output;
        readonly IDoubleOutput _phase2Output;
        double[] _inPhaseDutyCycle;
        double[] _outOfPhaseDutyCycle;
        int _phaseIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepperMotor"/> class.
        /// </summary>
        /// <param name="phase1Output">The H-Bridge that controls motor phase 1.</param>
        /// <param name="phase2Output">The H-Bridge that controls motor phase 2.</param>
        /// <param name="stepsPerStepCycle">The steps per step cycle.</param>
        public StepperMotor(
            IDoubleOutput phase1Output,
            IDoubleOutput phase2Output,
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
                new[] { +1.0, +1.0, +0.0, -1.0, -1.0, -1.0, +0.0, +1.0 };

            _outOfPhaseDutyCycle =
                new[] { +0.0, +1.0, +1.0, +1.0, +0.0, -1.0, -1.0, -1.0 };
        }

        void ComputeWholeStepTables()
        {
            _inPhaseDutyCycle =
                new[] { +1.0, -1.0, -1.0, +1.0 };

            _outOfPhaseDutyCycle =
                new[] { +1.0, +1.0, -1.0, -1.0 };
        }

        public void ReleaseHoldingTorque()
        {
            _phase1Output.Value = 0.0;
            _phase2Output.Value = 0.0;
        }

        void ComputeMicrostepTables(int microsteps)
        {
            // This implementation prefers performance over memory footprint.
            var radiansPerIndex = (2 * Math.PI) / (microsteps - 1);
            _inPhaseDutyCycle = new double[microsteps];
            _outOfPhaseDutyCycle = new double[microsteps];
            for (var i = 0; i < microsteps; ++i)
            {
                var phaseAngle = i * radiansPerIndex;
                _inPhaseDutyCycle[i] = Math.Sin(phaseAngle);
                _outOfPhaseDutyCycle[i] = Math.Cos(phaseAngle);
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
