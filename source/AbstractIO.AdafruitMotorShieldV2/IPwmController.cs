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
// File: IPwmController.cs  Created: 2014-06-07@16:50
// Last modified: 2014-11-30@13:57 by Tim
namespace AbstractIO.AdafruitMotorShieldV2
{
    internal interface IPwmController
    {
        /// <summary>
        ///   Gets the output modulation frequency, in Hertz.
        ///   The output modulation frequency is the frequency with which the PWN channels complete
        ///   one on/off cycle.
        /// </summary>
        /// <value>The output modulation frequency in Hertz.</value>
        double OutputModulationFrequencyHz { get; }

        /// <summary>
        ///   Configures the specified PWM channel with the specified duty cycle.
        /// </summary>
        /// <param name="channel">The channel number (0-based).</param>
        /// <param name="dutyCycle">The duty cycle as a fraction of unity.</param>
        void ConfigureChannelDutyCycle(uint channel, double dutyCycle);

        PwmChannel GetPwmChannel(uint channel);
    }
}
