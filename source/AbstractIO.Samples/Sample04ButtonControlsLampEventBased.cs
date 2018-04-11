using System;
using System.Threading;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Controls a lamp using a button without polling but using events (stemming from IRQs).
    /// </summary>
    public static class Sample04ButtonControlsLampEventBased
    {
        /// <summary>
        /// The lamp to be controlled.
        /// </summary>
        private static IBooleanOutput _lamp;

        public static void Run(IObservableBooleanInput button, IBooleanOutput lamp)
        {
            // Check arguments:
            if (button == null) throw new ArgumentNullException(nameof(button));
            if (lamp == null) throw new ArgumentNullException(nameof(lamp));

            // Store the lamp so that the event handler can use it:
            _lamp = lamp;

            // Attach the event handler to the event that the button raises whenever it's Value changed:
            button.ValueChanged += ButtonValueChangedHandler;

            // Just wait and let the event handler react on button changes:
            for (; ; ) Thread.Sleep(1000);
        }

        /// <summary>
        /// Handles the <see cref="IObservableBooleanInput.ValueChanged"/> event.
        /// </summary>
        /// <param name="sender">The object which raised the event (that is, the button).</param>
        /// <param name="newValue">The new value to which the button's <see cref="IBooleanInput.Value"/> property
        /// changed.</param>
        private static void ButtonValueChangedHandler(object sender, bool newValue)
        {
            // Turn the lamp on or off if the button was pressed or released, respectively:
            _lamp.Value = newValue;
        }
    }
}
