using System;

namespace AbstractIO
{
    /// <summary>
    /// A class mapping an <see cref="IDoubleInput"/> to an <see cref="IBooleanInput"/> using a threshold value.
    /// </summary>
    public class DoubleSchmittTriggerInput : IBooleanInput
    {
        private IDoubleInput _sourceInput;
        private double _lowThreshold, _highThreshold;
        bool _currentValue;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInput">The input whose analog value shall be mapped to a boolean value.</param>
        /// <param name="threshold">The threshold above which <see cref="Value"/> shall return true, and below which
        /// it shall return false. This can by any number.</param>
        /// <param name="hysteresis">The range that the source value must be above/below the last value in order to
        /// change the resulting value. This must not be negative.</param>
        /// <remarks>Example: Use 0.5 as <paramref name="threshold"/> and 0.1 as <paramref name="hysteresis"/>. The
        /// following sequence of source input values, in the given order, will then result in the following resulting
        /// values: 0.0 -> false, 1.0 -> true, 0.6 -> true, 0.5 -> true, 0.4 -> true, 0.3 -> false, 0.4 -> false,
        /// 0.5 -> false, 0.6 -> false, 0.7 -> true.</remarks>
        public DoubleSchmittTriggerInput(IDoubleInput sourceInput, double threshold, double hysteresis)
        {
            _sourceInput = sourceInput ?? throw new ArgumentNullException(nameof(sourceInput));
            if (hysteresis < 0.0) { throw new ArgumentOutOfRangeException(nameof(hysteresis)); }

            hysteresis = hysteresis / 2.0;
            _lowThreshold = threshold - hysteresis;
            _highThreshold = threshold + hysteresis;
        }

        /// <summary>
        /// Gets (reads) the source value and returns the mapped boolean value.
        /// </summary>
        public bool Value
        {
            get
            {
                // Read the source value once:
                double sourceValue = _sourceInput.Value;

                // Check if the value to be returned must change:
                if (sourceValue < _lowThreshold)
                {
                    _currentValue = false;
                }
                else if (sourceValue > _highThreshold)
                {
                    _currentValue = true;
                }
                return _currentValue;
            }
        }
    }

}
