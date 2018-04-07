namespace AbstractIO
{
    /// <summary>
    /// An abstract output writing boolean values.
    /// </summary>
    public interface IBooleanOutput
    {
        /// <summary>
        /// Gets the last value written or sets the value to be written.
        /// </summary>
        bool Value { get; set; }
    }

    /// <summary>
    /// An abstract output writing integer values.
    /// </summary>
    public interface IIntegerOutput
    {
        /// <summary>
        /// Gets the last value written or sets the value to be written.
        /// </summary>
        int Value { get; set; }
    }

    /// <summary>
    /// An abstract output writing double values.
    /// </summary>
    public interface IDoubleOutput
    {
        /// <summary>
        /// Gets the last value written or sets the value to be written.
        /// </summary>
        double Value { get; set; }
    }
}
