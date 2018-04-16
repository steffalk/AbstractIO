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
// Copyright © 2014-2015 Tigra Astronomy, all rights reserved.
// This source code is licensed under the MIT License, see http://opensource.org/licenses/MIT
// 
// File: IStepSequencer.cs  Created: 2015-01-13@13:45
// Last modified: 2015-01-31@16:02 by Tim

namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    ///   Interface IStepSequencer - defines the capability to sequence windings in a stepper motor
    /// </summary>
    public interface IStepSequencer
    {
        /// <summary>
        ///   Configures the motor coils in such a way as to move the armature through one 'step'.
        ///   A 'step' in this context is defined as the smallest unit of movement that the motor is capable of
        ///   and may be a microstep, half step, full step or any other kind of step.
        /// </summary>
        /// <param name="direction">The direction, +1 for forwards, -1 for reverse, 0 for stop.</param>
        void PerformStep(int direction);

        /// <summary>
        ///   Releases the holding torque by de-energizing all windings.
        ///   This allows the motor to rotate freely.
        /// </summary>
        void ReleaseHoldingTorque();
    }
}
