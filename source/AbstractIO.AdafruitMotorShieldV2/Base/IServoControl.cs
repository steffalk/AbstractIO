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
// File: IServoControl.cs  Created: 2015-03-18@02:11
// Last modified: 2015-03-19@00:56 by Tim

namespace AbstractIO.AdafruitMotorShieldV2
{
    /// <summary>
    ///     Interface IServoControl - defines methods for controlling the position of a servo motor
    /// </summary>
    public interface IServoControl
    {
        /// <summary>
        ///     Gets or sets the position of the servo expressed as a fraction of unity, where 0.0 represents full clockwise
        ///     rotation and 1.0 represents full counterclockwise rotation.
        /// </summary>
        /// <value>The position expressed as a fraction of unity, i.e. in the range 0.0 to +1.0 inclusive.</value>
        float Position { get; set; }
        /// <summary>
        ///     Gets or sets the angular position (in positive degrees) of the servo motor. The zero-point is the position of maximum 
        ///     clockwise rotation and the angle is measured counter-clockwise.
        ///     The maximum angle is implementation dependent.
        /// </summary>
        /// <value>The angle, in positive degrees, away from full clockwise displacement.</value>
        float Angle { get; set; }
    }
}