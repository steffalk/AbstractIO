using System;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Demonstrates the WaitFor() and WaitForChange() methods polling <see cref="IBooleanInput"/> objects.
    /// Note that the exact same code works without polling, only by using <see cref="IObservableBooleanInput"/>
    /// objects, in <see cref="Sample07WaitForButtonEventBased"/>.
    /// </summary>
    public static class Sample06WaitForButtonPolling
    {
        /// <summary>
        /// Waits for a button to turn from false to true, then turns a lamp on, and after that turns the lamp on or off
        /// on every change of the button.
        /// </summary>
        /// <param name="button">The button to use.</param>
        /// <param name="lamp">The lamp to use.</param>
        public static void Run(IBooleanInput button, IBooleanOutput lamp)
        {
            // Check parameters:
            if (button == null) throw new ArgumentNullException(nameof(button));
            if (lamp == null) throw new ArgumentNullException(nameof(lamp));

            // Wait for the button to turn from false to true (test this holding the button when the program starts!):
            button.WaitFor(true, true);
            lamp.Value = true;

            // Wait for the button to change to any value and set the lamp accordingly.
            while (true)
            {
                lamp.Value = button.WaitForChange();
            }
        }
    }
}
