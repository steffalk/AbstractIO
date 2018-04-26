using System;

namespace AbstractIO
{
    /// <summary>
    /// A class inverting an <see cref="IBooleanOuput"/>.
    /// </summary>
    public class BooleanInvertedOutput : IBooleanOutput
    {
        private readonly IBooleanOutput _target;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="target">The ouput get the inverted Value of this object.</param>
        public BooleanInvertedOutput(IBooleanOutput target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Gets or sets the inverted Value of the target object passed to the constructor, or sets the target object's
        /// Value to the inverted Value of this object.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_target.Value;
            }
            set
            {
                _target.Value = !value;
            }
        }
    }
}
