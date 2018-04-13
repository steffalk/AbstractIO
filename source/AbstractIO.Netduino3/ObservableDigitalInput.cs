using Windows.Devices.Gpio;

namespace AbstractIO.Netduino3
{
    /// <summary>
    /// A ditigal input pin of a Netduino 3 board with reads done via event handling.
    /// </summary>
    public class ObservableDigitalInput : DigitalInputOutputBase, IObservableBooleanInput
    {
        /// <summary>
        /// The current <see cref="Value"/> of this object.
        /// </summary>
        private bool _Value;

        /// <summary>
        /// This event gets raised every time the <see cref="Value"/> property changed to a new, different value.
        /// </summary>
        public event BooleanValueChangedHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The arguments for the event.</param>
        protected void OnValueChanged(bool newValue)
        {
            ValueChanged(this, newValue);
        }

        /// <summary>
        /// Initializes the object, useable for all constructors.
        /// </summary>
        private void Init()
        {
            _Value = Pin.Read() == GpioPinValue.High;
            Pin.ValueChanged += PinValueChangedHandler;
        }

        /// <summary>
        /// Creates an instance using the default GpioController and a specific pin drive mode.
        /// </summary>
        /// <param name="pin">The pin to use.</param>
        /// <param name="driveMode">The mode of the pin. This must be valid for input.</param>
        public ObservableDigitalInput(DigitalInputPin pin, GpioPinDriveMode mode) :
            base((int)pin, DigitalInputOutputBase.CheckInputMode(mode))
        {
            Init();
        }

        /// <summary>
        /// Creates an instance using the default GpioController and <see cref="GpioPinDriveMode.InputPullDown"/>.
        /// </summary>
        /// <param name="pin">The pin to use.</param>
        public ObservableDigitalInput(DigitalInputPin pin) :
            base((int)pin, GpioPinDriveMode.InputPullDown)
        {
            Init();
        }

        /// <summary>
        /// Gets the last value of the input, determined either through an initial read operation or through the last
        /// occurence of the <see cref="ValueChanged"/> event.
        /// </summary>
        public bool Value
        {
            get
            {
                return _Value;
            }
        }

        /// <summary>
        /// Handles the <see cref="GpioPin.ValueChanged"/> event for the underlying pin.
        /// </summary>
        /// <param name="sender">The object raising the event (the underlying <see cref="GpioPin"/>).</param>
        /// <param name="e">The arguments for the event.</param>
        private void PinValueChangedHandler(object sender, GpioPinValueChangedEventArgs e)
        {
            bool newValue = e.Edge == GpioPinEdge.RisingEdge;
            if (newValue != _Value)
            {
                _Value = newValue;
                OnValueChanged(newValue);
            }
        }

        /// <summary>
        /// Disposes the underlying objects. This method will be called automatically and should not be called by user
        /// code, but inheritors should call the base method.
        /// </summary>
        protected override void DisposeResource()
        {
            if (Pin != null)
            {
                Pin.ValueChanged -= PinValueChangedHandler;
            }
            base.DisposeResource();
        }
    }
}
