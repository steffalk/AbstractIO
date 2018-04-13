// This file is part of the TA.NetMF.MotorControl project
// 
// Copyright © 2014-2015 Tigra Astronomy, all rights reserved.
// This source code is licensed under the MIT License, see http://opensource.org/licenses/MIT
// 
// File: AdafruitV2MotorShield.cs  Created: 2015-01-13@13:45
// Last modified: 2015-02-02@18:10 by Tim

using System;

namespace AbstractIO.AdafruitMotorShieldV2
{
    public class AdafruitV2MotorShield
    {
        readonly Pca9685PwmController pwmController;

        /// <summary>
        ///   Initializes a new instance of the <see cref="AdafruitV2MotorShield" /> class at the specified
        ///   I2C address.
        /// </summary>
        /// <param name="address">
        ///   The I2C base address of the shield (optional; defaults to 0x60).
        /// </param>
        public AdafruitV2MotorShield(ushort address = 0x60)
        {
            pwmController = new Pca9685PwmController(address);
        }

        public void InitializeShield() { }

        /// <summary>
        ///   Gets a stepper motor with the specified number of microsteps. The
        ///   phases specify which of the 4 motor outputs the stepper motor
        ///   windings are connected to. The outputs M1, M2, M3 and M4 can be
        ///   read from the silk screen of the shield.
        /// </summary>
        /// <param name="microsteps">The number of microsteps per stepping cycle. </param>
        /// <param name="phase1">The output number (M1, M2, M3 or M4) that the first motor phase is connected to.</param>
        /// <param name="phase2">The output number (M1, M2, M3 or M4) that the  second motor phase is connected to.</param>
        /// <returns>
        ///   An implementation of <see cref="IStepSequencer" />  that can control the specified motor windings in
        ///   microsteps.
        /// </returns>
        public IStepSequencer GetMicrosteppingStepperMotor(int microsteps, int phase1, int phase2)
        {
            if (phase1 > 4 || phase1 < 1)
                throw new ArgumentOutOfRangeException("phase1", "must be 1, 2, 3 or 4");
            if (phase2 > 4 || phase2 < 1)
                throw new ArgumentOutOfRangeException("phase2", "must be 1, 2, 3 or 4");
            if (phase1 == phase2)
                throw new ArgumentException("The motor phases must be on different outputs");
            var hbridge1 = GetHbridge(phase1);
            var hbridge2 = GetHbridge(phase2);
            var motor = new TwoPhaseMicrosteppingSequencer(hbridge1, hbridge2, microsteps);
            return motor;
        }

        /// <summary>
        ///   Gets a stepper motor controller that performs 4 whole steps per stepping cycle.
        ///   The phases specify which of the 4 motor outputs the stepper motor
        ///   windings are connected to. The outputs M1, M2, M3 and M4 can be
        ///   read from the silk screen of the shield.
        /// </summary>
        /// <param name="phase1">The output number (M1, M2, M3 or M4) that the first motor phase is connected to.</param>
        /// <param name="phase2">The output number (M1, M2, M3 or M4) that the  second motor phase is connected to.</param>
        /// <returns>
        ///   An implementation of <see cref="IStepSequencer" /> that can control the specified motor windings in whole
        ///   steps.
        /// </returns>
        public IStepSequencer GetFullSteppingStepperMotor(int phase1, int phase2)
        {
            return GetMicrosteppingStepperMotor(4, phase1, phase2);
        }

        /// <summary>
        ///   Gets a stepper motor controller that performs 8 half steps per stepping cycle.
        ///   The phases specify which of the 4 motor outputs the stepper motor
        ///   windings are connected to. The outputs M1, M2, M3 and M4 can be
        ///   read from the silk screen of the shield.
        /// </summary>
        /// <param name="phase1">The output number (M1, M2, M3 or M4) that the first motor phase is connected to.</param>
        /// <param name="phase2">The output number (M1, M2, M3 or M4) that the  second motor phase is connected to.</param>
        /// <returns>
        ///   An implementation of <see cref="IStepSequencer" /> that can control the specified motor windings in half
        ///   steps.
        /// </returns>
        public IStepSequencer GetHalfSteppingStepperMotor(int phase1, int phase2)
        {
            return GetMicrosteppingStepperMotor(8, phase1, phase2);
        }

        /// <summary>
        ///   Gets an H-Bridge instance for the specified motor output number.
        /// </summary>
        /// <param name="motorNumber">
        ///   The motor number, as printed on the silk screen of the Adafruit motor shield
        ///   (M1, M2, M3, M4).
        /// </param>
        /// <returns>
        ///   An H-Bridge configured with all of the correct PWM channels corresponding to the
        ///   specified motor number.
        /// </returns>
        HBridge GetHbridge(int motorNumber)
        {
            // Assumption: motorNumber has been validated elsewhere.
            // The channel numbers below tally with those used in Adafruit's driver.
            switch (motorNumber)
            {
                case 1:
                    return new PwmControlledHBridge(pwmController, 10, 9, 8);
                case 2:
                    return new PwmControlledHBridge(pwmController, 11, 12, 13);
                case 3:
                    return new PwmControlledHBridge(pwmController, 5, 6, 7);
                case 4:
                    return new PwmControlledHBridge(pwmController, 4, 3, 2);
                default:
                    throw new ArgumentOutOfRangeException("motorNumber", "Must be 1, 2, 3, or 4");
            }
        }

        /// <summary>
        ///   Gets a DC motor on the specified connector pins.
        /// </summary>
        /// <param name="connectorNumber">
        ///   The connector number, as indicated on the shield's silk
        ///   screen, where the motor will be connected.
        /// </param>
        /// <returns>
        ///   an <see cref="HBridge" /> instance configured to control the specified motor
        ///   connector.
        /// </returns>
        public HBridge GetDcMotor(int connectorNumber)
        {
            return GetHbridge(connectorNumber);
        }

        /// <summary>
        ///     Gets a servo motor.
        /// </summary>
        /// <param name="servoNumber">The servo number, which must be either 1 or 2 for this shield.</param>
        /// <returns>IServoControl.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">servoNumber;Valid servo numbers for this shield are 1 or 2.</exception>
        /// <remarks>Servo 1 uses PWM pin D10; Servo 2 uses PWM pin D9.</remarks>
        public IServoControl GetServoMotor(int servoNumber)
        {
            switch (servoNumber)
            {
                case 1:
                    return new ServoMotor(3 * 16 + 10); // PWMChannels.PWM_PIN_D10);
                case 2:
                    return new ServoMotor(3 * 16 + 9); // PWMChannels.PWM_PIN_D9);
                default:
                    throw new ArgumentOutOfRangeException("servoNumber",
                        "Valid servo numbers for this shield are 1 or 2.");
            }
        }

    }
}
