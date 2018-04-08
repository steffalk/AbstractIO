using Windows.Devices.Gpio;

namespace AbstractIO.Netduino3
{
    /// <summary>
    /// A digital input pin of a Netduino 3 board with reads done using polling only.
    /// </summary>
    public class DigitalInput : DigitalInputOutputBase, IBooleanInput
    {
        /// <summary>
        /// Creates an instance using the default GpioController and a specific pin drive mode.
        /// </summary>
        /// <param name="pin">The pin to use.</param>
        /// <param name="driveMode">The mode of the pin. This must be valid for input.</param>
        public DigitalInput(DigitalInputPin pin, GpioPinDriveMode mode) :
            base((int)pin, DigitalInputOutputBase.CheckInputMode(mode))
        {
        }

        /// <summary>
        /// Creates an instance using the default GpioController and <see cref="GpioPinDriveMode.InputPullDown"/>.
        /// </summary>
        /// <param name="pin">The pin to use.</param>
        public DigitalInput(DigitalInputPin pin) :
            base((int)pin, GpioPinDriveMode.InputPullDown)
        {
        }

        /// <summary>
        /// Actively reads the pin value.
        /// </summary>
        public bool Value
        {
            get
            {
                return Pin.Read() == GpioPinValue.High;
            }
        }
    }
}
