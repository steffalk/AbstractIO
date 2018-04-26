namespace AbstractIO
{
    /// <summary>
    /// An abstract input reading single-precision floating point values.
    /// </summary>
    public interface ISingleInput
    {
        /// <summary>
        /// Gets (reads) the value.
        /// </summary>
        float Value { get; }
    }
}
