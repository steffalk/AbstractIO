// This file is part of the TA.NetMF.MotorControl project
// 
// Copyright © 2014-2014 Tigra Astronomy, all rights reserved.
// This source code is licensed under the MIT License, see http://opensource.org/licenses/MIT
// 
// File: Pca9685PwmController.cs  Created: 2014-06-06@18:06
// Last modified: 2014-11-30@13:57 by Tim

using System;
using System.Threading;
using Math = System.Math;
using Windows.Devices.I2c;

namespace AbstractIO.AdafruitMotorShieldV2
{
    internal class Pca9685PwmController : IPwmController
    {
        private static Object _lockObject = new Object();

        const int MaxChannel = 15;
        const int PwmCounterCycle = 4096;
        readonly ushort i2cAddress;
        I2cConnectionSettings i2CConfiguration;
        I2cDevice i2cDevice;
        double outputModulationFrequencyHz;

        /// <summary>
        ///   Initializes a new instance of the <see cref="Pca9685PwmController" /> class at the specified I2C address
        ///   and with the specified output modulation frequency.
        /// </summary>
        /// <param name="i2cAddress">The base I2C address for the device.</param>
        /// <param name="outputModulationFrequencyHz">
        ///   The output modulation frequency of all 16 PWM channels, in Hertz (cycles per second).
        ///   If not specified, then the default value of 1.6 KHz is used. The theoretical range is
        ///   approximately 24 Hz to 1743 Hz, but extremes should be avoided if possible.
        /// </param>
        public Pca9685PwmController(ushort i2cAddress = 0x60,
            double outputModulationFrequencyHz = Pca9685.DefaultOutputModulationFrequency)
        {
            this.i2cAddress = i2cAddress;
            InitializeI2CDevice();
            Reset();
            SetOutputModulationFrequency(outputModulationFrequencyHz);
            // At this point the device is fully configured but all PWM channels are turned off.
        }

        public PwmChannel GetPwmChannel(uint channel)
        {
            if (channel > MaxChannel)
                throw new ArgumentOutOfRangeException("channel", "Maximum channel is 15");
            return new PwmChannel(this, channel, 0.0);
        }

        public double OutputModulationFrequencyHz { get { return outputModulationFrequencyHz; } }

        public void ConfigureChannelDutyCycle(uint channel, double dutyCycle)
        {
            if (dutyCycle >= 1.0)
            {
                SetFullOn(channel);
                return;
            }
            if (dutyCycle <= 0.0)
            {
                SetFullOff(channel);
                return;
            }
            uint onCount = 0;
            var offCount = (uint)Math.Floor(PwmCounterCycle * dutyCycle);
            if (offCount <= onCount)
                offCount = onCount + 1; // The two counts may not be the same value
            var registerOffset = (byte)(Pca9685.Channel0OnLow + (4 * channel));
            WriteConsecutiveRegisters(
                registerOffset,
                (byte)onCount,
                (byte)(onCount >> 8),
                (byte)offCount,
                (byte)(offCount >> 8));
        }

        /// <summary>
        ///   Sets the channel to 0% duty cycle.
        /// </summary>
        /// <param name="channel">The channel number (0-based).</param>
        void SetFullOff(uint channel)
        {
            var registerOffset = (byte)(Pca9685.Channel0OnLow + (4 * channel));
            WriteConsecutiveRegisters(registerOffset, 0x00, 0x00, 0x00, 0x00); // Set LED_FULL_OFF bit
        }

        /// <summary>
        ///   Sets the channel to 100% duty cycle.
        /// </summary>
        /// <param name="channel">The channel number (0-based).</param>
        void SetFullOn(uint channel)
        {
            var registerOffset = (byte)(Pca9685.Channel0OnLow + (4 * channel));
            WriteConsecutiveRegisters(registerOffset, 0x00, 0x10, 0x00, 0x00); // Set LED_FULL_ON bit
        }

        void InitializeI2CDevice()
        {
            i2CConfiguration = new I2cConnectionSettings(i2cAddress);
            i2cDevice = I2cDevice.FromId("IC21", i2CConfiguration);
        }

        void SetBitInRegister(byte registerOffset, ushort bitNumber)
        {
            var bitSetMask = 0x01 << bitNumber;
            var registerValue = (int)ReadRegister(registerOffset);
            registerValue |= bitSetMask;
            WriteRegister(registerOffset, (byte)registerValue);
        }

        void ClearBitInRegister(byte registerOffset, ushort bitNumber)
        {
            var bitClearMask = 0xFF ^ (0x01 << bitNumber);
            var registerValue = (int)ReadRegister(registerOffset);
            registerValue &= bitClearMask;
            WriteRegister(registerOffset, (byte)registerValue);
        }

        /// <summary>
        ///   Resets the PCA9685 PWM controller into a known starting state
        ///   and turns off all PWM channels. Blocks for at least 1 millisecond
        ///   to allow the internal oscillator to stabilize.
        /// </summary>
        public void Reset()
        {
            WriteRegister(Pca9685.Mode1Register, 0x00);
            Thread.Sleep(1);
            SetAllChannelsOff();
            // Set the RESATART bit, but only if necessary.
            if (BitIsClear(Pca9685.Mode1Register, Pca9685.RestartBit))
                return;
            SetBitInRegister(Pca9685.Mode1Register, Pca9685.RestartBit);
        }

        /// <summary>
        ///   Sets all channels to 0% duty cycle.
        /// </summary>
        void SetAllChannelsOff()
        {
            // Sets the LED_FULL_OFF bit in each and every channel, which sets the duty cycle to 0%.
            WriteConsecutiveRegisters(Pca9685.AllChannelsBaseRegister, 0x00, 0x00, 0x00, 0x10);
        }

        /// <summary>
        /// Sets the LED_FULL_ON bit in each and every channel, which sets the duty cycle to 100%..
        /// </summary>
        void SetAllChannelsOn()
        {
            WriteConsecutiveRegisters(Pca9685.AllChannelsBaseRegister, 0x00, 0x10, 0x00, 0x00);
        }

        void WriteRegister(byte registerOffset, byte data)
        {
            byte[] writeBuffer = { registerOffset, data };
            lock (_lockObject)
            {
                i2cDevice.Write(writeBuffer);
            }
            //var operations = new I2cDevice.I2CTransaction[1];
            //operations[0] = I2cDevice.CreateWriteTransaction(writeBuffer);
            //i2cDevice.Execute(operations, Pca9685.I2CTimeout);
        }

        void WriteConsecutiveRegisters(byte startRegisterOffset, params byte[] values)
        {
            if (BitIsClear(Pca9685.Mode1Register, Pca9685.AutoIncrementBit))
                SetAutoIncrement(true);
            var bufferSize = values.Length + 1;
            var writeBuffer = new byte[bufferSize];
            writeBuffer[0] = startRegisterOffset;
            var bufferIndex = 0;
            foreach (var value in values)
                writeBuffer[++bufferIndex] = value;
            lock (_lockObject)
            {
                i2cDevice.Write(writeBuffer);
            }
            //var transactions = new I2cDevice.I2CTransaction[]
            //    {
            //    I2cDevice.CreateWriteTransaction(writeBuffer)
            //    };
            //i2cDevice.Execute(transactions, Pca9685.I2CTimeout);
        }

        bool BitIsSet(byte registerOffset, ushort bitNumber)
        {
            var registerValue = ReadRegister(registerOffset);
            var bitTestMask = 0x01 << bitNumber;
            return (registerValue & bitTestMask) != 0;
        }

        public void SetOutputModulationFrequency(double frequencyHz = Pca9685.DefaultOutputModulationFrequency)
        {
            // See PCA9685 data sheet, pp.24 for details on calculating the prescale value.
            var computedPrescale = Math.Round(Pca9685.InternalOscillatorFrequencyHz / 4096.0 / frequencyHz) - 1;
            if (computedPrescale < 3.0 || computedPrescale > 255.0)
                throw new ArgumentOutOfRangeException("frequencyHz", "range 24 Hz to 1743 Hz");
            var prescale = (byte)computedPrescale;
            SetPrescale(prescale);
            outputModulationFrequencyHz = frequencyHz;
        }

        /// <summary>
        ///   Enables or disables the automatic increment mode.
        ///   When enabled, sequential register reads are possible.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        void SetAutoIncrement(bool enabled)
        {
            if (enabled)
                SetBitInRegister(Pca9685.Mode1Register, Pca9685.AutoIncrementBit);
            else
                ClearBitInRegister(Pca9685.Mode1Register, Pca9685.AutoIncrementBit);
        }

        /// <summary>
        ///   Sets the master prescale divider for all channels.
        ///   Thi will turn off all PWM channels. They must be reconfigured
        ///   after setting the prescale value.
        /// </summary>
        /// <param name="prescale">The prescale.</param>
        void SetPrescale(byte prescale)
        {
            // The prescaler can only be set while the device is in SLEEP mode.
            // so we must put it to sleep, set the prescaler, then restart it.
            byte sleep = (byte)(0x01 << Pca9685.SleepBit);  // Set the SLEEP bit
            WriteRegister(Pca9685.Mode1Register, sleep);
            WriteRegister(Pca9685.PrescaleRegister, prescale);
            Reset();
        }

        bool BitIsClear(byte registerOffset, ushort bitNumber)
        {
            return !BitIsSet(registerOffset, bitNumber);
        }

        byte ReadRegister(byte registerOffset)
        {
            byte[] writeBuffer = { registerOffset };
            var readBuffer = new byte[1];
            lock (_lockObject)
            {
                i2cDevice.Write(writeBuffer);
                i2cDevice.Read(readBuffer);
            }
            //var operations = new I2cDevice.I2CTransaction[2];
            //operations[0] = I2cDevice.CreateWriteTransaction(writeBuffer);
            //operations[1] = I2cDevice.CreateReadTransaction(readBuffer);
            //i2cDevice.Execute(operations, Pca9685.I2CTimeout);
            var result = readBuffer[0];
            return result;
        }
    }
}
