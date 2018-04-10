namespace AbstractIO
{
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
}
