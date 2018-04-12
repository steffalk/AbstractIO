using System;

namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IDoubleOutput"/> which scales the full range of values coming from a source
    /// <see cref="IDoubleInput"/> to a given target range. This class is self-learing, which means that the incoming
    /// range of values gets automatically expanded whenever an input value is outside of the so far learned range.
    /// </summary>
    /// <remarks>
    /// Say, you have an incoming analog value from a potentiometer or light sensor, and you want that to be linearly
    /// mapped to an output range from -1.0 to +1.0. This class will do that, and when the incoming value range is
    /// extended because values come outside the so far known range, the range from which the input gets mapped to the
    /// output is automatically extended. Example: For the first 10 seconds, all incoming values are between 0.2 and
    /// 0.3. Then 0.2 will get mapped to -1.0 and 0.3 to +1.0. If thereafter a value of 0.1 comes in, the input range
    /// 0.1 - 0.3 will get mapped to -1.0 to +1.0 in a linear fashion.
    /// </remarks>
    public class ScaleToRangeInput : IDoubleInput
    {
        private IDoubleInput _source;
        private double _smallestValueMappedTo, _largestValueMappedTo, _sourceMinimum, _sourceMaximum;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="source">The input whose value shall be mapped.</param>
        /// <param name="smallestValueMappedTo">The value that the smallest source input value will be mapped to.
        /// </param>
        /// <param name="largestValueMappedTo">The value that the largest source input value will be mapped to.</param>
        /// <remarks><paramref name="smallestValueMappedTo"/> may well be larger than
        /// <paramref name="largestValueMappedTo"/>. In this case the mapping will output a larger value for smaller
        /// source values, and you can, vor example, turn the sign of an input ranging from -1.0 to +1.0 into the
        /// resulting range +1.0 to -1.0. However, <paramref name="smallestValueMappedTo"/> must not be equal to
        /// <paramref name="largestValueMappedTo"/>.</remarks>
        public ScaleToRangeInput(IDoubleInput source, double smallestValueMappedTo, double largestValueMappedTo)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            if (smallestValueMappedTo == largestValueMappedTo)
            {
                throw new ArgumentException(
                    nameof(smallestValueMappedTo) + " must not be equal to " + nameof(largestValueMappedTo) + ".");
            }

            _smallestValueMappedTo = smallestValueMappedTo;
            _largestValueMappedTo = largestValueMappedTo;
            _sourceMinimum = double.MaxValue;
            _sourceMaximum = double.MinValue;
        }

        /// <summary>
        /// Gets (reads) the mapped value. It will always be in the range from <see cref="SmallestValueMappedTo"/> to
        /// <see cref="LargestValueMappedTo"/>.
        /// </summary>
        public double Value
        {
            get
            {
                // Read the source once:
                double sourceValue = _source.Value;

                // Auto-learn and extend the known source value range:
                if (sourceValue < _sourceMinimum) { _sourceMinimum = sourceValue; }
                if (sourceValue > _sourceMaximum) { _sourceMaximum = sourceValue; }

                // Compute the mapped value:
                double result;
                double interval = _sourceMaximum - _sourceMinimum;

                if (interval > 0.0)
                {
                    // We have a real source value range larger than zero. Interpolate:
                    result =
                        (sourceValue - _sourceMinimum)
                        / interval
                        * (_largestValueMappedTo - _smallestValueMappedTo) + _smallestValueMappedTo;
                }
                else
                {
                    // We have no range, as _sourceMaximum and _sourceMinimum are equal.
                    // Return the value in the middle of the desired output range.
                    result = (_largestValueMappedTo + _smallestValueMappedTo) / 2.0;
                }

                // Ensure that the computation result is within the desired output range, even if rounding errors
                // occurred, respecting positive or negative mapping:
                if (_smallestValueMappedTo < _largestValueMappedTo)
                {
                    result = Math.Min(Math.Max(result, _smallestValueMappedTo), _largestValueMappedTo);
                }
                else
                {
                    result = Math.Min(Math.Max(result, _largestValueMappedTo), _smallestValueMappedTo);
                }

                return result;
            }
        }
    }
}
