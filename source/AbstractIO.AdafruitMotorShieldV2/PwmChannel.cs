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
// File: PwmChannel.cs  Created: 2014-06-07@15:58
// Last modified: 2014-11-30@13:57 by Tim

using System;

namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    ///   Class PwmChannel. Represents a single channel of the PCA9685 PWM controller.
    ///   Attribution: This code is based (albeit in a simplified form) on Microsoft.SPOT.Hardware.PWM
    ///   Note: the fundamental oscillator frequency is determined by the parent PWM Controller and cannot be overridden here.
    /// </summary>
    public sealed class PwmChannel
    {
        public enum ScaleFactor : uint
        {
            Milliseconds = 1000U,
            Microseconds = 1000000U,
            Nanoseconds = 1000000000U,
        }

        private readonly uint _channel; // The base address of the channel's registers.

        private uint _duration; // The duration for which the PWM signal is active.
        private uint _period; // The PWM waveform period, 1/frequency
        private ScaleFactor _scale;
        private IPwmController _controller;
        private double _dutyCycle;

        internal PwmChannel(IPwmController controller, uint channel, double dutyCycle)
        {
            if (dutyCycle < 0.0 || dutyCycle > 1.0)
            {
                throw new ArgumentOutOfRangeException("dutyCycle", "must be a fraction of unity");
            }
            _channel = channel;
            _controller = controller;
            _dutyCycle = dutyCycle;
            Commit();
        }

        internal PwmChannel(IPwmController controller, uint channel, uint period, uint duration, ScaleFactor scale)
        {
            _channel = channel;
            _period = period;
            _duration = duration;
            _scale = scale;
            _controller = controller;
            Commit();
        }

        public double DutyCycle
        {
            get
            {
                return _dutyCycle;
            }
            set
            {
                if (value < 0.0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException("value", "must be a fraction of unity");
                }
                this._dutyCycle = value;
                Commit();
            }
        }

        /// <summary>
        /// Commits the configured duty cycle to the PWM hardware.
        /// </summary>
        private void Commit()
        {
            _controller.ConfigureChannelDutyCycle(this._channel, this._dutyCycle);
        }

    }
}
