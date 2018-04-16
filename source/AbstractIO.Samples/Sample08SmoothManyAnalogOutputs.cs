using System;
using System.Threading;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Controls many analog outputs in a smoothed way as a demo.
    /// </summary>
    public static class Sample08SmoothManyAnalogOutputs
    {
        /// <summary>
        /// Lets many analog outputs smoothly increase or decrease positive and negative power as a demo.
        /// </summary>
        /// <param name="outputs"></param>
        public static void Run(params IDoubleOutput[] outputs)
        {
            // Check parameters:
            if (outputs == null || outputs.Length == 0) { throw new ArgumentNullException(nameof(outputs)); }
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == null)
                {
                    throw new ArgumentException("outputs must not contain empty elements.");
                }
            }

            // Generate smoothed versions of the outputs:
            IDoubleOutput[] smoothedOutputs = new IDoubleOutput[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                smoothedOutputs[i] = outputs[i].Smoothed(valueChangePerSecond: 0.25, rampIntervalMs: 20);
            }

            // Do the demo:

            var random = new Random();

            const int PauseImMs = 5000;
            const double MinimumSpeed = 0.3;

            while (true)
            {
                // Full positive power to all outputs:
                foreach (IDoubleOutput output in smoothedOutputs)
                {
                    output.Value = 1.0;
                }
                Thread.Sleep(2 * PauseImMs);

                // Full negative power to all outputs:
                foreach (IDoubleOutput output in smoothedOutputs)
                {
                    output.Value = -1.0;
                }
                Thread.Sleep(4 * PauseImMs);

                // Several cycles of random power:
                for (int cycle = 0; cycle < 10; cycle++)
                {
                    foreach (IDoubleOutput output in smoothedOutputs)
                    {
                        double value = random.NextDouble() * (2.0 - 2.0 * MinimumSpeed) - 1.0 + MinimumSpeed;
                        if (value < 0)
                        {
                            value -= MinimumSpeed;
                        }
                        else
                        {
                            value += MinimumSpeed;
                        }
                        output.Value = value;
                    }
                    Thread.Sleep(PauseImMs);
                }
            }
        }
    }
}
