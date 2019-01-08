using System;

namespace AbstractIO
{
    /// <summary>
    /// A class for debouncing a <see cref="IBooleanInput"/> to give a new <see cref="IBooleanInput"/> which changes its
    /// value not faster than after a given time span after the last change.
    /// </summary>
    public class BooleanDebouncedInput : IBooleanInput
    {
        private IBooleanInput _inputToDebounce;
        private int _debounceMilliseconds;
        private DateTime _nextMeasurementTime = DateTime.MinValue;
        private bool _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="inputToDebounce">The input to debounce, for example a switch.</param>
        /// <param name="debounceMilliseconds">The number of milliseconds that value which was read from
        /// <paramref name="inputToDebounce"/> shall be returned unchanged by the resulting <see cref="IBooleanInput"/>,
        /// even if the source value changes (due to bouncing effects, say, on a mechanical switch).</param>
        public BooleanDebouncedInput(IBooleanInput inputToDebounce, int debounceMilliseconds)
        {
            _inputToDebounce = inputToDebounce ?? throw new ArgumentNullException(nameof(inputToDebounce));
            if (debounceMilliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(debounceMilliseconds));
            }
            _debounceMilliseconds = debounceMilliseconds;
        }

        /// <summary>
        /// Gets the debounced value of the source <see cref="IBooleanInput"/>.
        /// </summary>
        public bool Value
        {
            get
            {
                var now = DateTime.UtcNow;
                if (now >= _nextMeasurementTime)
                {
                    _value = _inputToDebounce.Value;
                    _nextMeasurementTime = now.AddMilliseconds(_debounceMilliseconds);
                }
                return _value;
            }
        }
    }
}
