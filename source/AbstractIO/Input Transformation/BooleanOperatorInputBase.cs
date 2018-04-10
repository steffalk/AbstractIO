using System;

namespace AbstractIO
{
    /// <summary>
    /// The base class for <see cref="IBooleanInput"/> objects combining other <see cref="IBooleanInput"/> objecs with
    /// binary operators.
    /// </summary>
    public abstract class BooleanOperatorInputBase : IBooleanInput
    {
        /// <summary>
        /// The source inputs to be operated on.
        /// </summary>
        private IBooleanInput[] _sourceInputs;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInputs">The inputs to be operated on.</param>
        protected BooleanOperatorInputBase(params IBooleanInput[] sourceInputs)
        {
            if (sourceInputs == null || sourceInputs.Length == 0)
            {
                throw new ArgumentNullException(nameof(sourceInputs));
            }
            foreach (var input in sourceInputs)
            {
                if (input == null)
                {
                    throw new ArgumentException("sourceInputs must not contain empty elements.");
                }
            }
            _sourceInputs = new IBooleanInput[sourceInputs.Length];
            sourceInputs.CopyTo(_sourceInputs, 0);
        }

        /// <summary>
        /// Gets the source inputs to be operated on.
        /// </summary>
        protected IBooleanInput[] SourceInputs
        {
            get
            {
                return _sourceInputs;
            }
        }

        /// <summary>
        /// Gets (reads) the value.
        /// </summary>
        public abstract bool Value { get; }
    }
}
