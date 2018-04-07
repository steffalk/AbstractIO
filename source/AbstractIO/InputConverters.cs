using System;

namespace AbstractIO
{
    #region Inverter

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

    #endregion

    #region Extension methods

    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language.
    /// </summary>
    public static class InputConverterExtensionMethods
    {
        /// <summary>
        /// Creates a <see cref="BooleanInputInverter"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IBooleanInput Invert(this IBooleanInput source)
        {
            return new BooleanInputInverter(source);
        }
    }

    #endregion
}
