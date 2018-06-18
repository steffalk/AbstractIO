using System;
using System.Threading;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Controls many analog outputs in a smoothed way as a demo.
    /// </summary>
    public static class Sample08SmoothManyAnalogOutputs
    {

        private delegate float ValueGetter();

        /// <summary>
        /// Lets many analog outputs smoothly increase or decrease positive and negative power as a demo.
        /// </summary>
        /// <param name="outputs"></param>
        public static void Run(IBooleanOutput[] statusLeds, params ISingleOutput[] outputs)
        {
            // Check parameters:
            if (outputs == null || outputs.Length == 0) throw new ArgumentNullException(nameof(outputs));

            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == null)
                {
                    throw new ArgumentException("outputs must not contain empty elements.");
                }
            }

            // Generate smoothed versions of the outputs:
            SingleSmoothedOutput[] smoothedOutputs = new SingleSmoothedOutput[outputs.Length];
            int _numberOfAcceleratingMotors = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                smoothedOutputs[i] = outputs[i].Smoothed(valueChangePerSecond: 0.15f, rampIntervalMs: 50);

                // Register an event handler showing if 1, 2 or more motors are still accelerating by lighting a LED:
                smoothedOutputs[i].IsTargetReached.ValueChanged += (sender, value) =>
                {
                    if (value)
                    {
                        Interlocked.Decrement(ref _numberOfAcceleratingMotors);
                    }
                    else
                    {
                        Interlocked.Increment(ref _numberOfAcceleratingMotors);
                    }
                    for (int ledIndex = 0; ledIndex < statusLeds.Length; ledIndex++)
                    {
                        statusLeds[ledIndex].Value = _numberOfAcceleratingMotors > ledIndex;
                    }
                };
            }

            // Do the demo:

            var random = new Random();

            while (true)
            {
                // Full positive power to all outputs:
                SetOutputsAndWait(smoothedOutputs, () => +1.0f);

                // Full negative power to all outputs:
                SetOutputsAndWait(smoothedOutputs, () => -1.0f);

                // Several cycles of random power:
                for (int cycle = 0; cycle < 10; cycle++)
                {
                    SetOutputsAndWait(smoothedOutputs, () => (float)random.NextDouble() * 2.0f - 1.0f);
                }
            }
        }

        private static void SetOutputsAndWait(SingleSmoothedOutput[] smoothedOutputs, ValueGetter getValue)
        {
            // Set the target values:
            foreach (SingleSmoothedOutput output in smoothedOutputs)
            {
                output.Value = getValue();
            }

            // Wait for all target values to be reached:
            foreach (SingleSmoothedOutput output in smoothedOutputs)
            {
                output.IsTargetReached.WaitFor(true);
            }

            // Let the motors run at the target speed for a little while:
            Thread.Sleep(3000);
        }
    }
}
