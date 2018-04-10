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
}
