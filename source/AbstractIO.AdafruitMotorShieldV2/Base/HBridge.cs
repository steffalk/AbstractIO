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
// File: HBridge.cs  Created: 2015-01-13@13:45
// Last modified: 2015-02-02@14:55 by Tim

using AbstractIO;
using System;

namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    ///   Class HBridge. Represents an H-Bridge motor driver with polarity and power control. An
    ///   H-bridge allows the direction of current flow to be controlled (polarity) and allows the
    ///   current to be turn on or off (enable). By using a PWM waveform generator at the enable
    ///   pin, the power output can be varied by varying the PWN duty cycle. Thus, one H-bridge can
    ///   control a single motor winding, be that a DC motor or one phase of a stepper motor.
    /// </summary>
    public abstract class HBridge : ISingleOutput
    {
        private float _duty;

        public bool Polarity { get { return (_duty >= 0); } }
        public float Power { get { return Math.Abs(_duty); } }

        /// <summary>
        /// Gets the last value written or sets the value to be written.
        /// </summary>
        public float Value
        {
            get
            {
                return _duty;
            }
            set
            {
                SetOutputPowerAndPolarity(Math.Min(Math.Max(value, -1.0f), 1.0f));
            }
        }

        /// <summary>
        ///   Sets the output power and polarity of the H-bridge.
        /// </summary>
        /// <param name="duty">
        ///   The output power expressed as a fraction, in the range -1.0 to +1.0 inclusive.
        ///   Positive values represent normal polarity and negative values represent reversed
        ///   polarity.
        /// </param>
        /// <remarks>
        ///   No attempt is made to define the sense of the polarity, it is merely 'one way or the
        ///   other'. The actual sign of voltages or current produced at the motor are dependent on
        ///   the application.
        /// </remarks>
        public virtual void SetOutputPowerAndPolarity(float duty)
        {
            if (duty > 1.0 || duty < -1.0)
                throw new ArgumentOutOfRangeException("duty", "-1.0 to 1.0 inclusive");
            this._duty = duty;
        }

        /// <summary>
        ///   Applies an induction brake to the motor winding. This generally results in the winding
        ///   being shorted (short brake) which allows the motor's internally generated magnetic field
        ///   to resist its own motion. Some devices are also capable of active braking by applying
        ///   rapidly changing reverses of current. The actual technique used is dependent on the
        ///   motor driver hardware and shield driver in use. Shields that do not implement braking
        ///   should simply not override this method, in which case applying the brake will be
        ///   equivalent to setting the velocity to 0.0 and letting the motor idle to a stop.
        /// </summary>
        public virtual void ApplyBrake()
        {
            SetOutputPowerAndPolarity(0.0f);
        }

        /// <summary>
        /// Releases the motor torque such that the motor is no longer driven and can idle freely.
        /// This is generally achieved by completely disabling the motor driver circuit.
        /// </summary>
        public virtual void ReleaseTorque()
        {
            SetOutputPowerAndPolarity(0.0f);
        }
    }
}
