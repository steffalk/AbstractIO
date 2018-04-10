namespace AbstractIO
{
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
