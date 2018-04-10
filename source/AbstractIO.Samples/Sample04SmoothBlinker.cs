using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample04SmoothBlinker
    {
        public static void Run(IDoubleOutput lamp)
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
                    lamp.Value = (double)step / (double)Steps;
                    Thread.Sleep(PauseInMs);
                }
                for (int step = Steps; step > 0; step--)
                {
                    lamp.Value = (double)step / (double)Steps;
                    Thread.Sleep(PauseInMs);
                }
            }
        }
    }
}
