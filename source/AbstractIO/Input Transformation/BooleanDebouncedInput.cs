using System;

namespace AbstractIO
{
    /// <summary>
    /// A class for debouncing a <see cref="IBooleanInput"/> to give a new <see cref="IBooleanInput"/> which changes its
    /// value not faster than after a given time span after the last change.
    /// </summary>
    public class BooleanDebouncedInput : IBooleanInput
    {
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
            this.InputToDebounce = inputToDebounce ?? throw new ArgumentNullException(nameof(inputToDebounce));
            if (debounceMilliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(debounceMilliseconds));
            }
            _debounceMilliseconds = debounceMilliseconds;
        }

        /// <summary>
        /// Gets the input to be debounced.
        /// </summary>
        public IBooleanInput InputToDebounce { get; }

        /// <summary>
        /// Gets or sets the time, in milliseconds, that the <see cref="Value"/> property will return an unchanged value
        /// after the <see cref="InputToDebounce"/> changed its value, even if the input changes its value during this
        /// time.
        /// </summary>
        public int DebounceMilliseconds
        {
            get
            {
                return _debounceMilliseconds;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(DebounceMilliseconds));
                }
                else
                {
                    _debounceMilliseconds = value;
                }
            }
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
                    _value = InputToDebounce.Value;
                    _nextMeasurementTime = now.AddMilliseconds(DebounceMilliseconds);
                }
                return _value;
            }
        }
    }
}
