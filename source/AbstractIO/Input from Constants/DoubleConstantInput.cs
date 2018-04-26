namespace AbstractIO
{
    /// <summary>
    /// An input always returning the same Double value.
    /// </summary>
    public class DoubleConstantInput : IDoubleInput
    {
        private readonly double _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">The value that the <see cref="Value"/> property shall return.</param>
        public DoubleConstantInput(double value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns the value passed to the constructor.
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
        }
    }
}