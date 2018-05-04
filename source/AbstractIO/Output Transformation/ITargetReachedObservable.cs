namespace AbstractIO
{
    /// <summary>
    /// An interface for objects which can get whether some target state is reached or not yet reached, and raise an
    /// event when this status changes.
    /// </summary>
    public interface ITargetReachedObservable
    {
        /// <summary>
        /// Gets an <see cref="IObservableBooleanInput"/> returning if some desired target state is reached and which
        /// will raise events when this status changes.
        /// </summary>
        IObservableBooleanInput IsTargetReached { get; }
    }
}
