using System;

namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IDoubleOutput"/> that passes its value linear scaled to a target <see cref="IDoubleOutput"/>.
    /// </summary>
    public class ScaledOutput : IDoubleOutput
    {
        private IDoubleOutput _target;
        private double _factor, _offset, _value;

        /// <summary>
        /// Creates an instance given a factor and an offset.
        /// </summary>
        /// <param name="target">The target output which shall receive the scaled values.</param>
        /// <param name="factor">The factor to scale the output.</param>
        /// <param name="offset">The offset to add to the output.</param>
        /// <remarks>Setting the <see cref="Value"/> will set the <paramref name="target"/> value to
        /// <see cref="Value"/> * <paramref name="factor"/> + <paramref name="offset"/>.</remarks>
        public ScaledOutput(IDoubleOutput target, double factor, double offset)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            _target = target;
            _factor = factor;
            _offset = offset;
        }

        /// <summary>
        /// Creates an instance given a factor only, using 0.0 as the offset.
        /// </summary>
        /// <param name="target">The target output which shall receive the scaled values.</param>
        /// <param name="factor">The factor to scale the output.</param>
        /// <remarks>Setting the <see cref="Value"/> will set the <paramref name="target"/> value to
        /// <see cref="Value"/> * <paramref name="factor"/>.</remarks>
        public ScaledOutput(IDoubleOutput target, double factor)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            _target = target;
            _factor = factor;
        }

        /// <summary>
        /// Gets the last value written or sets the value to be written. It will be passed to the target
        /// <see cref="IDoubleOutput.Value">Value</see> scaled as defined by the constructor parameters.
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _target.Value = value * _factor + _offset;
            }
        }

    }
}
