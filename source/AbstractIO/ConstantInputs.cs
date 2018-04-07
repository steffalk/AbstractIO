namespace AbstractIO
{
    /// <summary>
    /// An input always returning the same Boolean value.
    /// </summary>
    public class ConstantBooleanInput : IBooleanInput
    {
        private readonly bool _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">The value that the <see cref="Value"/> property shall return.</param>
        public ConstantBooleanInput(bool value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns the value passed to the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return _value;
            }
        }
    }

    /// <summary>
    /// An input always returning the same Integer value.
    /// </summary>
    public class ConstantIntegerInput : IIntegerInput
    {
        private readonly int _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">The value that the <see cref="Value"/> property shall return.</param>
        public ConstantIntegerInput(int value)
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

    /// <summary>
    /// An input always returning the same Double value.
    /// </summary>
    public class ConstantDoubleInput : IDoubleInput
    {
        private readonly double _value;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">The value that the <see cref="Value"/> property shall return.</param>
        public ConstantDoubleInput(double value)
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