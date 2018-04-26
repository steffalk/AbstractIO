namespace AbstractIO
{
    /// <summary>
    /// An input always returning the same float value.
    /// </summary>
    public class SingleConstantInput : ISingleInput
    {
        private readonly float _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">The value that the <see cref="Value"/> property shall return.</param>
        public SingleConstantInput(float value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns the value passed to the constructor.
        /// </summary>
        public float Value
        {
            get
            {
                return _value;
            }
        }
    }
}