using System.Threading;

namespace AbstractIO.Netduino3.Samples
{
    /// <summary>
    /// Demonstrates simple digital output by blinking the onboard LED.
    /// </summary>
    internal static class Sample01SimpleBlinker
    {
        /// <summary>
        /// Runs the sample.
        /// </summary>
        public static void Run()
        {
            // Create the led. This is the only place where the physical output is important.
            IBooleanOutput led = new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue);

            // Run the blinker. Note that only the members of the IBooleanOutput interface are used, regardless of what
            // actually gets driven by that output.
            while (true)
            {
                led.Value = !led.Value;
                Thread.Sleep(500);
            }
        }
    }
}
