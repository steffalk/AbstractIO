// This file is part of the TA.NetMF.MotorControl project
// 
// Copyright © 2014-2014 Tigra Astronomy, all rights reserved.
// This source code is licensed under the MIT License, see http://opensource.org/licenses/MIT
// 
// File: PwmChannel.cs  Created: 2014-06-07@15:58
// Last modified: 2014-11-30@13:57 by Tim

using System;
using System.Collections;

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

        static readonly ArrayList AllocatedChannels = new ArrayList();

        readonly uint channel; // The base address of the channel's registers.

        uint duration; // The duration for which the PWM signal is active.
        uint period; // The PWM waveform period, 1/frequency
        ScaleFactor scale;
        IPwmController controller;
        double dutyCycle;

        internal PwmChannel(IPwmController controller, uint channel, double dutyCycle)
        {
            if (dutyCycle < 0.0 || dutyCycle > 1.0)
                throw new ArgumentOutOfRangeException("dutyCycle", "must be a fraction of unity");
            this.channel = channel;
            this.controller = controller;
            this.dutyCycle = dutyCycle;
            try
            {
                Init();
                Commit();
            }
            catch
            {
                Dispose(false);
            }
        }

        internal PwmChannel(IPwmController controller, uint channel, uint period, uint duration, ScaleFactor scale)
        {
            this.channel = channel;
            this.period = period;
            this.duration = duration;
            this.scale = scale;
            this.controller = controller;
            try
            {
                AllocateChannel(channel);
                Init();
                Commit();
            }
            catch
            {
                Dispose(false);
            }
        }

        public double DutyCycle
        {
            get { return dutyCycle; }
            set
            {
                if (value < 0.0 || value > 1.0) throw new ArgumentOutOfRangeException("value", "must be a fraction of unity");
                this.dutyCycle = value;
                Commit();
            }
        }




        /// <summary>
        ///   Allocates the channel after ensuring that it is not already in use.
        ///   Channel instances must be disposed before they can be re-used.
        /// </summary>
        /// <param name="channel">The channel base address.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is already an instance of this channel.</exception>
        void AllocateChannel(uint channel)
        {
            if (AllocatedChannels.Contains(channel))
                throw new InvalidOperationException("Channel with base address " + channel + " is already allocated");
            AllocatedChannels.Add(channel);
        }

        ~PwmChannel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            try
            { }
            finally
            {
                Uninit();
            }
        }

        /// <summary>
        /// Commits the configured duty cycle to the PWM hardware.
        /// </summary>
        private void Commit()
        {
            controller.ConfigureChannelDutyCycle(this.channel, this.dutyCycle);
        }


        private void Init()
        {
            AllocateChannel(channel);
        }

        private void Uninit()
        {
            DeallocateChannel();
        }

        void DeallocateChannel()
        {
            if (AllocatedChannels != null && AllocatedChannels.Contains(channel))
                AllocatedChannels.Remove(channel);
        }
    }
}
