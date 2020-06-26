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
// File: Pca9685.cs  Created: 2014-06-07@15:01
// Last modified: 2014-11-30@13:57 by Tim
namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    ///   Pca9685 constants.
    /// </summary>
    internal static class Pca9685Constants
    {
        #region MODE1 register bits
        public const ushort AllCallBit = 0;
        public const ushort AutoIncrementBit = 5;
        public const ushort ExtClockBit = 6;
        public const ushort RestartBit = 7;
        public const ushort SleepBit = 4;
        public const ushort Sub1Bit = 3;
        public const ushort Sub2Bit = 2;
        public const ushort Sub3Bit = 1;
        #endregion MODE1 register bits

        #region LED Control Register Bits
        public const ushort FullOffBit = 4;
        public const ushort FullOnBit = 4;
        #endregion LED Control Register Bits

        #region Register addresses
        public const byte AllChannelsBaseRegister = 0xFA;
        public const byte AllChannelsOffHigh = 0xFD;
        public const byte AllChannelsOffLow = 0xFC;
        public const byte AllChannelsOnHigh = 0xFB;
        public const byte AllChannelsOnLow = 0xFA;
        public const byte Channel0OffHigh = 0x09;
        public const byte Channel0OffLow = 0x08;
        public const byte Channel0OnHigh = 0x07;
        public const byte Channel0OnLow = 0x06;
        public const byte ChannelBase = 0x06;
        public const byte Mode1Register = 0x00;
        public const byte Mode2Register = 0x01;
        public const byte PrescaleRegister = 0xFE;
        #endregion Register addresses

        #region Other constants
        public const int ClockRateKhz = 100;
        public const int DefaultOutputModulationFrequency = 1600;
        public const int I2CTimeout = 3000; // milliseconds
        public const int InternalOscillatorFrequencyHz = 25000000;
        #endregion Other constants
    }
}
