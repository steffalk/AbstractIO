using System;

namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IBooleanInput"/> which copies the value of a source input to an tee target
    /// <see cref="IBooleanOutput"/> each time it's <see cref="IBooleanInput.Value">Value</see> property gets read. This
    /// may be useful, for example, if you want to monitor some boolean input by setting a lamp to its value each time
    /// it gets read.
    /// </summary>
    public class BooleanMonitoredInput : IBooleanInput
    {
        private IBooleanInput _sourceInput;
        private IBooleanOutput _teeTarget;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInput">The input to tee.</param>
        /// <param name="teeTarget">The output to receive the passed-through value of the
        /// <paramref name="sourceInput"/>.</param>
        public BooleanMonitoredInput(IBooleanInput sourceInput, IBooleanOutput teeTarget)
        {
            _sourceInput = sourceInput ?? throw new ArgumentNullException(nameof(sourceInput));
            _teeTarget = teeTarget ?? throw new ArgumentNullException(nameof(teeTarget));
        }

        /// <summary>
        /// Gets the <see cref="IBooleanInput.Value">Value</see> of the source input and at the same time sets the tee
        /// target <see cref="IBooleanOutput"/> to that same value.
        /// </summary>
        public bool Value
        {
            get
            {
                var value = _sourceInput.Value;
                _teeTarget.Value = value;
                return value;
            }
        }
    }
}
