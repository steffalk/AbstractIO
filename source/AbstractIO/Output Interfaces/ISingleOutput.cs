namespace AbstractIO
{
    /// <summary>
    /// An abstract output writing float values.
    /// </summary>
    public interface ISingleOutput
    {
        /// <summary>
        /// Gets the last value written or sets the value to be written.
        /// </summary>
        float Value { get; set; }
    }
}
