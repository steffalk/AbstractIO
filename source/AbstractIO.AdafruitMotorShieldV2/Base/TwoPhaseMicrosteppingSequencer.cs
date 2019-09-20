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

namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    /// Class TwoPhaseMicrosteppingSequencer. Provides an implementation of <see cref="IStepSequencer"/> that can perform
    /// stepsPerStepCycle of variable resolution, half steps or full steps.
    /// </summary>
    public class TwoPhaseMicrosteppingSequencer : IStepSequencer
    {
        readonly int maxIndex;
        readonly HBridge phase1;
        readonly HBridge phase2;
        float[] inPhaseDutyCycle;
        float[] outOfPhaseDutyCycle;
        int phaseIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoPhaseMicrosteppingSequencer"/> class.
        /// </summary>
        /// <param name="phase1bridge">The H-Bridge that controls motor phase 1.</param>
        /// <param name="phase2bridge">The H-Bridge that controls motor phase 2.</param>
        /// <param name="stepsPerStepCycle">The steps per step cycle.</param>
        public TwoPhaseMicrosteppingSequencer(HBridge phase1bridge, HBridge phase2bridge, int stepsPerStepCycle)
        {
            phase1 = phase1bridge;
            phase2 = phase2bridge;
            maxIndex = stepsPerStepCycle - 1;
            phaseIndex = 0;
            ConfigureStepTables(stepsPerStepCycle);
        }

        void ConfigureStepTables(int microsteps)
        {
            if (microsteps == 4)
            {
                ComputeWholeStepTables();
                return;
            }
            else if (microsteps == 8)
            {
                ComputeHalfStepTables();
                return;
            }
            else if (microsteps >= 8)
            {
                ComputeMicrostepTables(microsteps);
                return;
            }
            throw new ArgumentException("Use 4 for full steps; 8 for half steps; or >8 for stepsPerStepCycle", "microsteps");
        }

        void ComputeHalfStepTables()
        {
            inPhaseDutyCycle = new[]
                {+1.0f, +1.0f, +0.0f, -1.0f, -1.0f, -1.0f, +0.0f, +1.0f};
            outOfPhaseDutyCycle = new[]
                {+0.0f, +1.0f, +1.0f, +1.0f, +0.0f, -1.0f, -1.0f, -1.0f};
        }

        void ComputeWholeStepTables()
        {
            inPhaseDutyCycle = new[]
                {+1.0f, -1.0f, -1.0f, +1.0f};
            outOfPhaseDutyCycle = new[]
                {+1.0f, +1.0f, -1.0f, -1.0f};
        }

        public void PerformStep(int direction)
        {
            phaseIndex += direction;
            if (phaseIndex > maxIndex)
                phaseIndex = 0;
            if (phaseIndex < 0)
                phaseIndex = maxIndex;
            phase1.SetOutputPowerAndPolarity(inPhaseDutyCycle[phaseIndex]);
            phase2.SetOutputPowerAndPolarity(outOfPhaseDutyCycle[phaseIndex]);
        }

        public void ReleaseHoldingTorque()
        {
            phase1.SetOutputPowerAndPolarity(0.0f);
            phase2.SetOutputPowerAndPolarity(0.0f);
        }

        void ComputeMicrostepTables(int microsteps)
        {
            // This implementation prefers performance over memory footprint.
            var radiansPerIndex = (2 * (float)System.Math.PI) / (microsteps - 1);
            inPhaseDutyCycle = new float[microsteps];
            outOfPhaseDutyCycle = new float[microsteps];
            for (var i = 0; i < microsteps; ++i)
            {
                var phaseAngle = i * radiansPerIndex;
                inPhaseDutyCycle[i] = System.Math.Sin(phaseAngle);
                outOfPhaseDutyCycle[i] = System.Math.Cos(phaseAngle);
            }
        }
    }
}
