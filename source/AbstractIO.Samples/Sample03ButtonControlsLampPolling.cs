using System;
using System.Threading;

namespace AbstractIO.Samples
{
    /// <summary>
    /// This sample queries any boolean input (say, a button), and turns on any boolean output (say, a lamp) when the
    /// button is pressed. This sample uses polling the input (there is another sample using IRQ events).
    /// </summary>
    public static class Sample03ButtonControlsLampPolling
    {
        public static void Run(IBooleanInput button, IBooleanOutput lamp)
        {
            // Check parameters:
            if (button == null) throw new ArgumentNullException(nameof(button));
            if (lamp == null) throw new ArgumentNullException(nameof(lamp));

            // Control the lamp by the button, using polling:
            while (true)
            {
                lamp.Value = button.Value;
                Thread.Sleep(100); // Only to give you a chance to redeploy usung firmware as of 2018-04-08.
            }
        }
    }
}
