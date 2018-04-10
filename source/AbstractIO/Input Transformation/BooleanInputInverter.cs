using System;

namespace AbstractIO
{
    /// <summary>
    /// A class inverting an <see cref="IBooleanInput"/>.
    /// </summary>
    public class BooleanInputInverter : IBooleanInput
    {
        private readonly IBooleanInput _source;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="source">The input to be converted.</param>
        public BooleanInputInverter(IBooleanInput source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Gets the inverted value of the source input passed to the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_source.Value;
            }
        }
    }
}
