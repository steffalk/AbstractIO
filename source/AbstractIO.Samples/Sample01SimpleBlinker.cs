using System;
using System.Threading;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Demonstrates simple digital output by blinking any output, for example, an LED.
    /// </summary>
    public static class Sample01SimpleBlinker
    {
        /// <summary>
        /// Blinks any <see cref="IBooleanOutput"/>.
        /// </summary>
        /// <param name="lamp">The output to "blink".</param>
        public static void Run(IBooleanOutput lamp)
        {
            // Check parameters:
            if (lamp == null) throw new ArgumentNullException(nameof(lamp));

            // Blink the passed-in lamp.
            // Note that we do not need to know anything about the actual physical output used:
            while (true)
            {
                lamp.Value = !lamp.Value;
                Thread.Sleep(500);
            }
        }
    }
}
