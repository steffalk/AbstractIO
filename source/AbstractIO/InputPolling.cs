namespace AbstractIO
{
    /// <summary>
    /// An abstract input reading boolean values.
    /// </summary>
    public interface IBooleanInput
    {
        /// <summary>
        /// Gets (reads) the value.
        /// </summary>
        bool Value { get; }
    }

    /// <summary>
    /// An abstract input reading integer values.
    /// </summary>
    public interface IIntegerInput
    {
        /// <summary>
        /// Gets (reads) the value.
        /// </summary>
        int Value { get; }
    }

    /// <summary>
    /// An abstract input reading double values.
    /// </summary>
    public interface IDoubleInput
    {
        /// <summary>
        /// Gets (reads) the value.
        /// </summary>
        double Value { get; }
    }
}
