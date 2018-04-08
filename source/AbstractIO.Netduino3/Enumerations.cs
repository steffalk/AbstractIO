namespace AbstractIO.Netduino3
{
    #region Info

    // PWM on Netduino will be available for the following, if not conflicting with other use of the timers.
    // The Arduiino pins D0 - D13 are connected to MCU pins using timer x for PWM
    // D0  = PC7  = TIM8, channel 1
    // D1  = PC6  = TIM8, channel 0
    // D2  = PA3  = TIM2, channel 3 or TIM5, channel 3
    // D3  = PA2  = TIM2, channel 2 or TIM5, channel 2
    // D4  = PB12
    // D5  = PB8  = TIM4, channel 2 
    // D6  = PB9  = TIM4, channel 3
    // D7  = PA1  = TIM2, channel 1 or TIM5, channel 1 
    // D8  = PA0  = TIM2, channel 0 or TIM5, channel 0
    // D9  = PE5  
    // D10 = PB10 = TIM2, channel 2
    // D11 = PB15
    // D12 = PB14
    // D13 = PB13
    // D14 = GND
    // D15 = intentionally left blank
    // D16 = PB7  = I2C1 SDA = Data
    // D17 = PB6  = I2C1 SCL = Clock
    // Onboard button = PB5 (= PD11)
    //
    // A0 = PC0 = ADC1 channel 0/1
    // A1 = PC1 = ADC1 channel 0/1
    // A2 = PC2 = ADC2 channel 0/1
    // A3 = PC3 = ADC2 channel 0/1
    // A4 = PC4 = ADC3 channel 0/1
    // A5 = PC5 = ADC3 channel 0/1

    // Furthermore some LEDs available on the board:
    //
    // On board LED   = Blue  = PA10 = TIM1 channel 2
    // Go port 1 LED  = Blue  = PE9  = TIM1 channel 0
    // Go port 2 LED  = Blue  = PE11 = TIM1 channel 1
    // Go port 3 LED  = Blue  = PE14 = TIM1 channel 3
    // Power on LED   = White = PC13 = No timer, so no PWM
    // Wifi State LED = Yellow = PA8  = TIM1 channel 0
    // Wifi link LED  = Green  = PC9  = TIM8 channel 3

    #endregion

    /// <summary>
    /// The digital input pins of a Netduino 3 board.
    /// </summary>
    public enum DigitalInputPin
    {
        // The Arduino/Netduino pins D0 - D13 are connected to the following MCU pins, and using timer x for PWM:
        // D0  = PC7  = TIM8, channel 1
        // D1  = PC6  = TIM8, channel 0
        // D2  = PA3  = TIM2, channel 3 or TIM5, channel 3
        // D3  = PA2  = TIM2, channel 2 or TIM5, channel 2
        // D4  = PB12
        // D5  = PB8  = TIM4, channel 2 
        // D6  = PB9  = TIM4, channel 3
        // D7  = PA1  = TIM2, channel 1 or TIM5, channel 1 
        // D8  = PA0  = TIM2, channel 0 or TIM5, channel 0
        // D9  = PE5  
        // D10 = PB10 = TIM2, channel 2
        // D11 = PB15
        // D12 = PB14
        // D13 = PB13

        D0 = 2 * 16 + 7,
        D1 = 2 * 16 + 6,
        D2 = 0 * 16 + 3,
        D3 = 0 * 16 + 2,
        D4 = 1 * 16 + 12,
        D5 = 1 * 16 + 8,
        D6 = 1 * 16 + 9,
        D7 = 0 * 16 + 1,
        D8 = 0 * 16 + 0,
        D9 = 4 * 16 + 5,
        D10 = 1 * 16 + 10,
        D11 = 1 * 16 + 15,
        D12 = 1 * 16 + 14,
        D13 = 1 * 16 + 13,

        OnboardButton = 1 * 16 + 5
    }

    /// <summary>
    /// The digital output pins of a Netduino 3 board.
    /// </summary>
    /// <remarks>
    /// This is just the <see cref="DigitalInputPin"/> enumeration, but without the onboard button and extended with
    /// members for the onboard-LEDs.
    /// </remarks>
    public enum DigitalOutputPin
    {
        // Plain copies of the DigitalInputPin members:
        D0 = 2 * 16 + 7,
        D1 = 2 * 16 + 6,
        D2 = 0 * 16 + 3,
        D3 = 0 * 16 + 2,
        D4 = 1 * 16 + 12,
        D5 = 1 * 16 + 8,
        D6 = 1 * 16 + 9,
        D7 = 0 * 16 + 1,
        D8 = 0 * 16 + 0,
        D9 = 4 * 16 + 5,
        D10 = 1 * 16 + 10,
        D11 = 1 * 16 + 15,
        D12 = 1 * 16 + 14,
        D13 = 1 * 16 + 13,

        // The onboard LED:
        OnboardLedBlue = 0 * 16 + 10,

        // The LEDs next to the 3 GoBus ports:
        GoPort1Led = 4 * 16 + 9,
        GoPort2Led = 4 * 16 + 11,
        GoPort3Led = 4 * 16 + 14,

        // The Power LED:
        PowerLed = 2 * 16 + 13
    }

    /// <summary>
    /// The digital output pins of a Netduino 3 board which can be used with PWM.
    /// </summary>
    /// <remarks>
    /// This is a subset of the the <see cref="DigitalOutputPin"/> enumeration.
    /// </remarks>
    public enum DigitalPwmOutputPin
    {
        // Plain copies of the DigitalInputPin members, but commented out the ones not usable for PWM:
        D0 = 2 * 16 + 7,
        D1 = 2 * 16 + 6,
        D2 = 0 * 16 + 3,
        D3 = 0 * 16 + 2,
        // D4 = 1 * 16 + 12, // no PWM
        D5 = 1 * 16 + 8,
        D6 = 1 * 16 + 9,
        D7 = 0 * 16 + 1,
        D8 = 0 * 16 + 0,
        // D9 = 4 * 16 + 5, // no PWM
        // D10 = 1 * 16 + 10, // D10 would always be the same percentage as D3.
        // D11 = 1 * 16 + 15, // no PWM
        // D12 = 1 * 16 + 14, // no PWM
        // D13 = 1 * 16 + 13, // no PWM

        // The onboard LED:
        OnboardLedBlue = 0 * 16 + 10,

        // The LEDs next to the 3 GoBus ports:
        GoPort1Led = 4 * 16 + 9,
        GoPort2Led = 4 * 16 + 11,
        GoPort3Led = 4 * 16 + 14

        // The Power LED:
        // PowerLed = 2 * 16 + 13 // no PWM
    }

    /// <summary>
    /// The 6 analog ADC inputs of a Netduino 3 board.
    /// </summary>
    public enum AnalogInputPin
    {
        // DO NOT CHANGE THE ORDER OF THESE MEMBERS!
        // DO NOT CHANGE THE VALUES OF THESE MEMBERS!
        // They must correspond to the channel numbers to pass to AdcController.OpenChannel().
        // Otherwise, don't forget to adapt the Netduino3.AdcInput constructor.

        A0,
        A1,
        A2,
        A3,
        A4,
        A5,

        // SENSOR
        InternalTemperatureSensor,

        // VREFINT
        InternalReferenceVoltage,

        // VBAT
        Battery
    }
}