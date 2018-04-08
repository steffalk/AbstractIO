using Windows.Devices.Adc;

namespace AbstractIO.Netduino3
{
    /// <summary>
    /// An analog ADC input of a Netduino 3 board.
    /// </summary>
    public class AdcInput : DisposableResourceBase, IDoubleInput
    {
        /// <summary>
        /// The ADC controller used by this object.
        /// </summary>
        private readonly AdcController _controller;

        /// <summary>
        /// The ADC channel on the used controller used by this object.
        /// </summary>
        private AdcChannel _channel;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="pin">The ADC input pin to be read by this object.</param>
        public AdcInput(AnalogInputPin pin)
        {
            _controller = AdcController.GetDefault();
            _channel = _controller.OpenChannel((int)pin);
        }

        /// <summary>
        /// Reads and gets the current value of the ADC channel as a value between 0.0 and 1.0.
        /// </summary>
        public double Value
        {
            get
            {
                return _channel.ReadRatio();
            }
        }

        /// <summary>
        /// Disposes the underlying objects. This method will be called automatically and should not be called by user
        /// code.
        /// </summary>
        protected override void DisposeResource()
        {
            if (_channel != null)
            {
                _channel.Dispose();
                _channel = null;
            }
        }
    }
}
