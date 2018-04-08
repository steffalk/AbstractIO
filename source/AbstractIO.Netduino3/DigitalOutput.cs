using Windows.Devices.Gpio;

namespace AbstractIO.Netduino3
{
    public class DigitalOutput : DigitalInputOutputBase, IBooleanOutput
    {
        /// <summary>
        /// The current <see cref="Value"/> of this object.
        /// </summary>
        private bool _value;

        /// <summary>
        /// Creates an instance using a specific GpioController and pin drive mode.
        /// </summary>
        /// <param name="controller">The controller to use.</param>
        /// <param name="pin">The pin to use.</param>
        /// <param name="driveMode">The mode of the pin. This must be valid for output.</param>
        public DigitalOutput(GpioController controller, DigitalOutputPin pin, GpioPinDriveMode mode) :
            base(controller, (int)pin, DigitalInputOutputBase.CheckOutputMode(mode))
        {
        }

        /// <summary>
        /// Creates an instance using the default GpioController and a specific pin drive mode.
        /// </summary>
        /// <param name="pin">The pin to use.</param>
        /// <param name="driveMode">The mode of the pin. This must be valid for output.</param>
        public DigitalOutput(DigitalOutputPin pin, GpioPinDriveMode mode) :
            base((int)pin, DigitalInputOutputBase.CheckOutputMode(mode))
        {
        }

        /// <summary>
        /// Creates an instance using the default GpioController and <see cref="GpioPinDriveMode.Output"/>.
        /// </summary>
        /// <param name="pin">The pin to use.</param>
        public DigitalOutput(DigitalOutputPin pin) :
            base((int)pin, GpioPinDriveMode.Output)
        {
        }

        /// <summary>
        /// Gets or sets the output value.
        /// </summary>
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                Pin.Write(value ? GpioPinValue.High : GpioPinValue.Low);
                _value = value;
            }
        }
    }
}
