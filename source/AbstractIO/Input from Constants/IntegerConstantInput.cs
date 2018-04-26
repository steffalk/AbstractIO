namespace AbstractIO
{
    /// <summary>
    /// An input always returning the same Integer value.
    /// </summary>
    public class IntegerConstantInput : IIntegerInput
    {
        private readonly int _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">The value that the <see cref="Value"/> property shall return.</param>
        public IntegerConstantInput(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns the value passed to the constructor.
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }
        }
    }
}