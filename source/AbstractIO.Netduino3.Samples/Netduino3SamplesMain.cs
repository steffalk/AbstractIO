// Please uncomment exactly one of the offered samples to run it:

#define Sample01SimpleBlinker
//#define Sample01SimpleBlinkerDistributed
//#define Sample01SimpleBlinkerAlternating
//#define Sample02SmoothPwmBlinker
//#define Sample03ButtonControlsLampPolling
//#define Sample03ButtonControlsLampPollingInvertingButton
//#define Sample03ButtonControlsLampPollingInvertingLamp
//#define Sample03ButtonControlsLampUsing2ButtonsWithAnd
//#define Sample03ButtonControlsLampUsing2ButtonsWithOr
//#define Sample03ButtonControlsLampBlinking
//#define Sample03ButtonControlsLampBlinkingSmoothly
//#define Sample04ButtonControlsLampEventBased
//#define Sample04ButtonControlsLampEventBasedInvertingButton
//#define Sample04ButtonControlsLampEventBasedSmoothly
//#define Sample05ControlLampBrightnessThroughAnalogInput
//#define Sample05ControlLampBrightnessThroughAnalogInputScaled
//#define Sample05ControlLampBrightnessThroughAnalogInputScaledInverted
//#define Sample06WaitForButtonPolling
//#define Sample07WaitForButtonEventBased

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
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort2Led).Inverted(),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.GoPort3Led),
                    new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue).Inverted()));

#elif Sample02SmoothPwmBlinker

            // Sample 02: Let a lamp blink smoothly. The abstract code just expects any IDoubleOutput and will cyle that
            // in small steps from 0.0 to 1.0 and back to 0.0 forever. As an example of an IDoubleOutput, we pass a
            // PWM-controlled pin:

            AbstractIO.Samples.Sample02SmoothBlinker.Run(
                lamp: new Netduino3.AnalogPwmOutput(DigitalPwmOutputPin.OnboardLedBlue));

#elif Sample03ButtonControlsLampPolling

            // Sample 03: Control a LED using a button:

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample03ButtonControlsLampPollingInvertingButton

            // Sample 03 again, but this time inverting the button simply by using a BooleanInputConverter, simply by
            // using the fluent API offered by the corresponding extension methods:

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(DigitalInputPin.OnboardButton).Invert(),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample03ButtonControlsLampPollingInvertingLamp

            // Sample 03 again, but this time inverting the lamp simply by using a BooleanOuputConverter, simply by
            // using the fluent API offered by the corresponding extension methods:

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue).Inverted());

#elif Sample03ButtonControlsLampUsing2ButtonsWithAnd

            // Sample 03 again, but this time the lamp shall only light up if both of two buttons are pressed.
            // To use this sample, connect two closing buttons to the Netduino 3 input pins D0 and D1 with their other
            // ports connected to VSS (+5V).

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new BooleanAndInput(
                    new Netduino3.DigitalInput(Netduino3.DigitalInputPin.D0),
                    new Netduino3.DigitalInput(Netduino3.DigitalInputPin.D1)),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample03ButtonControlsLampUsing2ButtonsWithOr

            // Sample 03 again, but this time the lamp shall light up if one or both of two buttons are pressed.
            // To use this sample, connect two closing buttons to the Netduino 3 input pins D0 and D1 with their other
            // ports connected to VSS (+5V).

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new BooleanOrInput(
                    new Netduino3.DigitalInput(Netduino3.DigitalInputPin.D0),
                    new Netduino3.DigitalInput(Netduino3.DigitalInputPin.D1)),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample03ButtonControlsLampBlinking

            // Sample 03 again, but this time we let the lamp blink simply by using the EnableableBlinker class, coded
            // using the fluent API provided by extension methods:

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue)
                        .BlinkedWhenTrue(onDurationMs: 300, offDurationMs: 500));

#elif Sample03ButtonControlsLampBlinkingSmoothly

            // Sample 03 again, but this time we let the lamp blink smoothly by using PWM and the SmoothOutput class,
            // coded using fluent API (even if the Run() method does nothing than simply turn the "output" on when the
            // button is pressed). Read the definition of the lamp in reverse order:
            // - Incoming is simply the boolean signal of the button.
            // - This is made blink (BlinkedWhenTrue).
            // - This, still boolean, value gets mapped to the double number 0.0 for false and 1.0 for true
            //   (MappedFromBoolean).
            // - This signal, which switches between 0.0 and 1.0, is then smoothed to slowly enlight or dimm the lamp
            //   (Smoothed).
            // - This, finally, is fed into the AnalogPwmOutput controlling the LED.
            // So, using the fluent API for outputs is coded from back to front: Define the target output (here, the
            // PWM-controlled LED, that is an IDoubleOutput), and apply transformations until you get an "I(type)Output"
            // output where "(type)" matches the output type expected (here, an IBooleanOutput).

            AbstractIO.Samples.Sample03ButtonControlsLampPolling.Run(
                button: new Netduino3.DigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.AnalogPwmOutput(Netduino3.DigitalPwmOutputPin.OnboardLedBlue)
                        .Smoothed(valueChangePerSecond: 1.0, rampIntervalMs: 20)
                        .MappedFromBoolean(falseValue: 0.0, trueValue: 1.0)
                        .BlinkedWhenTrue(onDurationMs: 300, offDurationMs: 500));

#elif Sample04ButtonControlsLampEventBased

            // Sample 04: Control a lamp using a button, but this time do not poll the status of the button, but react
            // to the ValueChanged event (that is, reacting on an IRQ generated by the µC whenever the status of the
            // button's input pin changed).

            AbstractIO.Samples.Sample04ButtonControlsLampEventBased.Run(
                button: new Netduino3.ObservableDigitalInput(DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample04ButtonControlsLampEventBasedInvertingButton

            // Sample 04 again: Control a lamp using a button using events, but with an inverted button using the
            // ObserverableBooleanInputInverter class, coded using the fluent API that the extension methods offer.

            AbstractIO.Samples.Sample04ButtonControlsLampEventBased.Run(
                button: new Netduino3.ObservableDigitalInput(DigitalInputPin.OnboardButton).Invert(),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample04ButtonControlsLampEventBasedSmoothly

            // Sample 04 again, but let the LED blink smoothly just as in Sample03ButtonControlsLampBlinkingSmoothly.
            // We do the very same here: Convert the IBooleanOutput of the event based method to the smoothly blinking
            // IDoubleOutput for the PWM-controlled LED. It works all the same whether we use a polling IBooleanInput
            // or the IRQ/event-based variant IObservableBooleanInput for the button. The output possibillities are just
            // the same.

            AbstractIO.Samples.Sample04ButtonControlsLampEventBased.Run(
                button: new Netduino3.ObservableDigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.AnalogPwmOutput(Netduino3.DigitalPwmOutputPin.OnboardLedBlue)
                    .Smoothed(valueChangePerSecond: 1.0, rampIntervalMs: 20)
                    .MappedFromBoolean(falseValue: 0.0, trueValue: 1.0)
                    .BlinkedWhenTrue(onDurationMs: 300, offDurationMs: 500));

#elif Sample05ControlLampBrightnessThroughAnalogInput

            // Sample 05: Let a LED light up just as bright (in the range from 0.0 to 1.0) as an analog input gives
            // values (also in the range from 0.0 to 1.0). Note that the input range is not scaled in any way in this
            // sample, but just goes straigt to the output. To run this sample, connect a variable resistor (such as a
            // photo cell) between anlog input pin A0 and GND (0V). Then, the lamp will light darker as more light goes
            // to the photo cell.

            AbstractIO.Samples.Sample05ControlLampBrightnessThroughAnalogInput.Run(
                input: new Netduino3.AnalogAdcInput(Netduino3.AnalogInputPin.A0),
                lamp: new Netduino3.AnalogPwmOutput(Netduino3.DigitalPwmOutputPin.OnboardLedBlue));

#elif Sample05ControlLampBrightnessThroughAnalogInputScaled

            // Sample 05 again, but this time auto-learn the actual incoming value range of the input and scale it to
            // the range from 0.0 to 1.0 using the ScaleToRangeInput class, coded using the fluent API of the
            // corresponding extension methods. This will cause the full range from 0.0 to 1.0 being used on the lamp,
            // regardless if, for example, the incoming values range only from 0.3 to 0.6. To run this sample, connect a
            // variable resistor (such as a photo cell) between anlog input pin A0 and GND (0V).

            AbstractIO.Samples.Sample05ControlLampBrightnessThroughAnalogInput.Run(
                input: new Netduino3.AnalogAdcInput(Netduino3.AnalogInputPin.A0)
                        .ScaleToRange(smallestValueMappedTo: 0.0, largestValueMappedTo: 1.0),
                lamp: new Netduino3.AnalogPwmOutput(Netduino3.DigitalPwmOutputPin.OnboardLedBlue));

#elif Sample05ControlLampBrightnessThroughAnalogInputScaledInverted

            // Sample 05 again, but this time the auto-learned ranged has swapped lower and upper limits. This results
            // in the lamp going brighter when the analog input signal gets lower (that is, the photo cell getting  more
            // light, and vice versa. To run this sample, connect a variable resistor (such as a photo cell) between
            // anlog input pin A0 and GND (0V).

            AbstractIO.Samples.Sample05ControlLampBrightnessThroughAnalogInput.Run(
                input: new Netduino3.AnalogAdcInput(Netduino3.AnalogInputPin.A0)
                        .ScaleToRange(smallestValueMappedTo: 1.0, largestValueMappedTo: 0.0),
                lamp: new Netduino3.AnalogPwmOutput(Netduino3.DigitalPwmOutputPin.OnboardLedBlue));

#elif Sample06WaitForButtonPolling

            // Sample 06: Wait for an input to reach a specific value, or to change, using the WaitFor() and
            // WaitForChange() extension methods.. In this sample, the button is used only as an IBooleanInput, so that
            // waiting will cause polling. See sample 07 for the exact same code, just using the butten as an
            // IObservableBooleanInput, working without polling.

            AbstractIO.Samples.Sample06WaitForButtonPolling.Run(
                button: new Netduino3.DigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#elif Sample07WaitForButtonEventBased

            // Sample 07: Nearly the same as sample 06, but using the button as an IObservableBooleanInput insted of
            // a plain IBooleanInput. This allows the WaitFor() and WaitForChange() methods to use IRQ events instead of
            // polling:

            AbstractIO.Samples.Sample07WaitForButtonEventBased.Run(
                button: new Netduino3.ObservableDigitalInput(Netduino3.DigitalInputPin.OnboardButton),
                lamp: new Netduino3.DigitalOutput(Netduino3.DigitalOutputPin.OnboardLedBlue));

#else
#error Please uncomment exactly one of the samples.
#endif
        }
    }
}
