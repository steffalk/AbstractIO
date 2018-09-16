using Windows.Devices.Adc;

namespace AbstractIO.Netduino3
{
    /// <summary>
    /// An analog ADC input of a Netduino 3 board.
    /// </summary>
    public class AnalogAdcInput : DisposableResourceBase, ISingleInput, IDoubleInput
    {
        /// <summary>
        /// The ADC channel on the used controller used by this object.
        /// </summary>
        private AdcChannel _channel;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="pin">The ADC input pin to be read by this object.</param>
        public AnalogAdcInput(AnalogInputPin pin)
        {
            _channel = AdcController.GetDefault().OpenChannel((int)pin);
        }

        /// <summary>
        /// Reads and gets the current value of the ADC channel as a value between 0.0 and 1.0.
        /// </summary>
        double IDoubleInput.Value
        {
            get
            {
                return _channel.ReadRatio();
            }
        }

        float ISingleInput.Value
        {
            get
            {
                return (float)_channel.ReadRatio();
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
