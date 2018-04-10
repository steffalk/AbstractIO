namespace AbstractIO
{
    /// <summary>
    /// The delegate for handlers of the <see cref="IObservableValue.ValueChanged"/> event.
    /// </summary>
    /// <param name="sender">The object which raised the event.</param>
    /// <param name="newValue">The new value to which the observed object changed.</param>
    /// <remarks>
    /// As these events might be wrapping IRQs and thus may occur very often, the signature of this handler does not
    /// have an EventArgs object, but carries the native new value as a parameter. This saves creating an EventArgs
    /// object on the heap every time this event gets fired, and thus can save many garbage collections. As this handler
    /// will pass all arguments on the stack and not by allocating objects on the heap, it should not cause garbage
    /// collections even if the event fires many times rapidly.
    /// </remarks>
    public delegate void IntegerValueChangedHandler(object sender, int newValue);

    /// <summary>
    /// An interface for <see cref="IIntegerInput"/> classes that can raise an event when the Value property changed.
    /// </summary>
    /// <remarks>
    /// This interface is intended, for example, for inputs which can react with an IRQ on changes and raise an event
    /// when this happens.
    /// </remarks>
    public interface IObservableIntegerInput : IIntegerInput
    {
        /// <summary>
        /// This event gets fired when the Value property of the abstract input/output interfaces has changed.
        /// </summary>
        /// <remarks>
        /// In addition to the Value property of the observed object, the new value to which that property changed will
        /// be readily passed to the newValue parameter of the event handler. Thus you have the guarantee to see the
        /// original value causing the event, not a possibly meanwhile again changed Value property. So, handlers of
        /// this event should usually inspect their newValue parameter and not query the object's Value property.
        /// </remarks>
        event IntegerValueChangedHandler ValueChanged;
    }
}
