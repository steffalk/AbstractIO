using System;

namespace AbstractIO
{
    #region Inverter

    /// <summary>
    /// A class inverting an <see cref="IBooleanInput"/>.
    /// </summary>
    public class BooleanInputInverter : IBooleanInput
    {
        private readonly IBooleanInput _source;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="source">The input to be converted.</param>
        public BooleanInputInverter(IBooleanInput source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Gets the inverted value of the source input passed to the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_source.Value;
            }
        }
    }

    /// <summary>
    /// A class inverting an <see cref="IObservableBooleanInput"/>.
    /// </summary>
    public class ObserverableBooleanInputInverter : IObservableBooleanInput
    {
        private readonly IObservableBooleanInput _source;

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
        /// <param name="newValue">The new value to which the input has changed.</param>
        protected void OnValueChanged(bool newValue)
        {
            ValueChanged(this, newValue);
        }

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="source">The input to be converted.</param>
        public ObserverableBooleanInputInverter(IObservableBooleanInput source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _source.ValueChanged += SourceValueChangedHandler;
        }

        /// <summary>
        /// Gets the inverted value of the source input passed to the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_source.Value;
            }
        }

        /// <summary>
        /// Handles the _source <see cref="IObservableBooleanInput.ValueChanged"/> event.
        /// </summary>
        /// <param name="sender">The object raising the event (that is, <see cref="_source"/>).</param>
        /// <param name="newValue">The new value to which <see cref="_source"/> has changed.</param>
        private void SourceValueChangedHandler(object sender, bool newValue)
        {
            // Raise the event with the inverted value.
            OnValueChanged(!newValue);
        }
    }

    #endregion

    #region Extension methods

    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language.
    /// </summary>
    public static class InputConverterExtensionMethods
    {
        /// <summary>
        /// Creates a <see cref="BooleanInputInverter"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IBooleanInput Invert(this IBooleanInput source)
        {
            return new BooleanInputInverter(source);
        }

        /// <summary>
        /// Creates a <see cref="IObservableBooleanInput"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IObservableBooleanInput Invert(this IObservableBooleanInput source)
        {
            return new ObserverableBooleanInputInverter(source);
        }

    }

    #endregion
}
