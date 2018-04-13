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
        double[] inPhaseDutyCycle;
        double[] outOfPhaseDutyCycle;
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
                {+1.0, +1.0, +0.0, -1.0, -1.0, -1.0, +0.0, +1.0};
            outOfPhaseDutyCycle = new[]
                {+0.0, +1.0, +1.0, +1.0, +0.0, -1.0, -1.0, -1.0};
        }

        void ComputeWholeStepTables()
        {
            inPhaseDutyCycle = new[]
                {+1.0, -1.0, -1.0, +1.0};
            outOfPhaseDutyCycle = new[]
                {+1.0, +1.0, -1.0, -1.0};
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
            phase1.SetOutputPowerAndPolarity(0.0);
            phase2.SetOutputPowerAndPolarity(0.0);
        }

        void ComputeMicrostepTables(int microsteps)
        {
            // This implementation prefers performance over memory footprint.
            var radiansPerIndex = (2 * Math.PI) / (microsteps - 1);
            inPhaseDutyCycle = new double[microsteps];
            outOfPhaseDutyCycle = new double[microsteps];
            for (var i = 0; i < microsteps; ++i)
            {
                var phaseAngle = i * radiansPerIndex;
                inPhaseDutyCycle[i] = Math.Sin(phaseAngle);
                outOfPhaseDutyCycle[i] = Math.Cos(phaseAngle);
            }
        }
    }
}
