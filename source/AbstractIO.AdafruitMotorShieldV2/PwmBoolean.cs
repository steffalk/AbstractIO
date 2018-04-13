// This file is part of the TA.NetMF.MotorControl project
// 
// Copyright © 2014-2014 Tigra Astronomy, all rights reserved.
// This source code is licensed under the MIT License, see http://opensource.org/licenses/MIT
// 
// File: PwmBoolean.cs  Created: 2014-09-28@04:12
// Last modified: 2014-11-30@13:57 by Tim
namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    ///   Class PwmBoolean.
    ///   An oddity of the Adafruit shield is that is uses most of the 16 PWM channels
    ///   as simple boolean outputs. This class wraps a PWM channel and makes it work
    ///   like a single bit on/off output.
    /// </summary>
    public class PwmBoolean
    {
        readonly PwmChannel pwm;

        /// <summary>
        ///   Initializes a new instance of the <see cref="PwmBoolean" /> class.
        /// </summary>
        /// <param name="pwm">
        ///   The PWM channel to be used as a boolean.
        ///   It is assume that the channel's frequency has already been initialized by its constructor.
        /// </param>
        public PwmBoolean(PwmChannel pwm, bool initialState = false)
        {
            this.pwm = pwm;
            InitializePwm(initialState);
        }

        /// <summary>
        ///   Gets or sets the state of this <see cref="PwmBoolean" />.
        /// </summary>
        /// <value><c>true</c> or <c>false</c>.</value>
        public bool State { get { return pwm.DutyCycle >= 1.0; } set { pwm.DutyCycle = value ? 1.0 : 0.0; } }

        /// <summary>
        ///   Initializes the PWM boolean to 'false' and starts it.
        /// </summary>
        void InitializePwm(bool initialState)
        {
            State = initialState;
        }
    }
}
