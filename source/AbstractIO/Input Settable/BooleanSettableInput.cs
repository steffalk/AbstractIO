namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IObservableBooleanInput"/> whose value can be freely set.
    /// </summary>
    public class BooleanSettableInput : IObservableBooleanInput
    {
        private bool _value;

        /// <summary>
        /// This event gets fired when the Value property of the abstract input/output interfaces has changed.
        /// </summary>
        /// <remarks>
        /// In addition to the Value property of the observed object, the new value to which that property changed will
        /// be readily passed to the newValue parameter of the event handler. Thus you have the guarantee to see the
        /// original value causing the event, not a possibly meanwhile again changed Value property. So, handlers of
        /// this event should usually inspect their newValue parameter and not query the object's Value property.
        /// </remarks>
        public event BooleanValueChangedHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="newValue">The new value to which the <see cref="Value"/> property changed.</param>
        protected void OnValueChanged(bool newValue)
        {
            ValueChanged(this, newValue);
        }

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="initialValue">The initial value for the <see cref="Value"/> property.</param>
        /// <remarks>
        /// The <see cref="ValueChanged"/> will not be raised until the first time the <see cref="Value"/> property has
        /// been changed after this object was created.
        /// </remarks>
        public BooleanSettableInput(bool initialValue )
        {
            _value = initialValue;
        }

        /// <summary>
        /// Gets or sets the value of the input. Setting it to another than its current value will raise the
        /// <see cref="ValueChanged"/> event.
        /// </summary>
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    OnValueChanged(value);
                }
            }
        }

    }
}
