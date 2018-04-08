// Uncomment exactly one of the offered samples:

#define Sample01SimpleBlinker
//#define Sample01SimpleBlinkerDistributed
//#define Sample01SimpleBlinkerAlternating
//#define Sample02ButtonControlsLampPolling
//#define Sample02ButtonControlsLampPollingInvertingButton
//#define Sample02ButtonControlsLampPollingInvertingLamp

namespace AbstractIO.Netduino3.Samples
{


    /// <summary>
    /// This class runs the abstract samples in AbstractIO.Samples on a Netduino 3 board.
    /// </summary>
    public static class Netduino3SamplesMain
    {
        /// <summary>
        /// Runs one of the abstract samples using physical ports of an Netduino 3 board.
        /// </summary>
        public static void Main()
        {
#if Sample01SimpleBlinker

            // Sample 01: Blink a LED:

            AbstractIO.Samples.Sample01SimpleBlinker.Run(
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample01SimpleBlinkerDistributed

            // Sample 01 again, but this time blinking several LEDs at once simply by distributing the output to them
            // using a BooleanOutputDistributor object, which on itself implements IBooleanOutput and simply passes the
            // Values to an arbitrary number of outputs:

            AbstractIO.Samples.Sample01SimpleBlinker.Run(
                lamp: new BooleanOutputDistributor(
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort1Led),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort2Led),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort3Led)));

#elif Sample01SimpleBlinkerAlternating

            // Sample 01 again, but this time blinking two LEDs alternating by using the BooleanOutputDistributor
            // combined with inverting one of the outputs using the BooleanOutputInverter, coded using the fluent API
            // that the corresponding extension method offers:

            AbstractIO.Samples.Sample01SimpleBlinker.Run(
                lamp: new BooleanOutputDistributor(
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort1Led),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort2Led).Invert(),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort3Led),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue).Invert()));

#elif Sample02ButtonControlsLampPolling

            // Sample 02: Control a LED using a button:

            AbstractIO.Samples.Sample02ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample02ButtonControlsLampPollingInvertingButton

            // Sample 02 again, but this time inverting the button simply by using a BooleanInputConverter, simply by
            // using the fluent API offered by the corresponding extension methods:

            AbstractIO.Samples.Sample02ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(DigitalInputPin.OnboardButton).Invert(),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample02ButtonControlsLampPollingInvertingLamp

            // Sample 02 again, but this time inverting the lamp simply by using a BooleanOuputConverter, simply by
            // using the fluent API offered by the corresponding extension methods:

            AbstractIO.Samples.Sample02ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue).Invert());

#else
#error Please uncomment exactly one of the samples.
#endif
        }
    }
}
