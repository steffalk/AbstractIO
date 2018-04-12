using System;
using System.Threading;

namespace AbstractIO
{
    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language. Input transformation extension methods are named, by convention, using a
    /// verb such as "Invert".
    /// </summary>
    public static class InputWaiterExtensionMethods
    {
        /// <summary>
        /// Pauses until an <see cref="IBooleanInput"/> returns a specified value, using polling.
        /// </summary>
        /// <param name="input">The input which shall be awaited.</param>
        /// <param name="value">The value that the input shall have before this method returns.</param>
        /// <remarks>
        /// This is a blocking method polling the <paramref name="input"/> value in short intervals.
        /// </remarks>
        public static void WaitFor(this IBooleanInput input, bool value)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            while (input.Value != value)
            {
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Pauses until an <see cref="IBooleanInput"/> changes its value and return the new value, using polling.
        /// </summary>
        /// <param name="input">The input which shall be awaited.</param>
        /// <returns>The new value of the input.</returns>
        public static bool WaitForChange(this IBooleanInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            bool oldValue = input.Value;
            bool newValue;

            while ((newValue = input.Value) != oldValue)
            {
                Thread.Sleep(1);
            }
            return newValue;
        }

        /// <summary>
        /// Pauses until an <see cref="IObservableBooleanInput"/> returns a specified value, using events (no polling).
        /// </summary>
        /// <param name="input">The input which shall be awaited.</param>
        /// <param name="value">The value that the input shall have before this method returns.</param>
        /// <param name="edgeOnly">If false, this method returns immediately if the desired <paramref name="value"/> is
        /// already present. If true, only a change from another value than <paramref name="value"/> to
        /// <paramref name="value"/> will cause the method to return.</param>
        /// <remarks>
        /// This is a blocking method.
        /// </remarks>
        public static void WaitFor(this IObservableBooleanInput input, bool value, bool edgeOnly)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // Only wait if the desired value is not already present and we do not wait for an edge:
            if (edgeOnly || (input.Value != value))
            {

                // The signal being set when the desired value is reached:
                ManualResetEvent valueReached = new ManualResetEvent(false);

                // Handles the input.ValueChanged event.
                void ValueChangedHandler(object sender, bool newValue)
                {
                    if (newValue == value)
                    {
                        valueReached.Set();
                    }
                }

                // Attach the event handler:
                input.ValueChanged += ValueChangedHandler;

                try
                {
                    // Wait (blocking) for the event handler to set the signal:
                    valueReached.WaitOne();
                }
                finally
                {
                    // Remove the event handler:
                    input.ValueChanged -= ValueChangedHandler;
                }
            }
        }

        /// <summary>
        /// Pauses until an <see cref="IObservableBooleanInput"/>  changes its value and return the new value, using
        /// events (no polling).
        /// </summary>
        /// <param name="input">The input which shall be awaited.</param>
        /// <param name="value">The value that the input shall have before this method returns.</param>
        /// <param name="edgeOnly">If false, this method returns immediately if the desired <paramref name="value"/> is
        /// already present. If true, only a change from another value than <paramref name="value"/> to
        /// <paramref name="value"/> will cause the method to return.</param>
        /// <remarks>
        /// This is a blocking method.
        /// </remarks>
        public static bool WaitForChange(this IObservableBooleanInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // The signal being set when the desired value is reached:
            ManualResetEvent valueReached = new ManualResetEvent(false);

            // Handles the input.ValueChanged event.
            void ValueChangedHandler(object sender, bool newValue)
            {
                valueReached.Set();
            }

            // Attach the event handler:
            input.ValueChanged += ValueChangedHandler;

            try
            {
                // Wait (blocking) for the event handler to set the signal:
                valueReached.WaitOne();
                return input.Value;
            }
            finally
            {
                // Remove the event handler:
                input.ValueChanged -= ValueChangedHandler;
            }
        }
    }
}
