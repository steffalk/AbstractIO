using System;
using System.Threading;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Lets a lamp blink smoothly by slowly increasing or decreasing its brightness.
    /// </summary>
    public static class Sample02SmoothBlinker
    {
        /// <summary>
        /// Runs the sample.
        /// </summary>
        /// <param name="lamp">The lamp, as an analog output.</param>
        public static void Run(ISingleOutput lamp)
        {
            // Check parameters:
            if (lamp == null) throw new ArgumentNullException(nameof(lamp));

            // Smoothly blink the output:

            const int Steps = 10;
            const int PauseInMs = 50;

            while (true)
            {
                for (int step = 0; step < Steps; step++)
                {
                    lamp.Value = (float)step / (float)Steps;
                    Thread.Sleep(PauseInMs);
                }
                for (int step = Steps; step > 0; step--)
                {
                    lamp.Value = (float)step / (float)Steps;
                    Thread.Sleep(PauseInMs);
                }
            }
        }
    }
}
