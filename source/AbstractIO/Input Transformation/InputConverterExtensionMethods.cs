using System.Threading;

namespace AbstractIO
{
    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language.
    /// </summary>
    public static class InputConverterExtensionMethods
    {
        /// <summary>
        /// Creates a <see cref="BooleanInputInverter"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IBooleanInput Invert(this IBooleanInput source)
        {
            return new BooleanInputInverter(source);
        }

        /// <summary>
        /// Creates a <see cref="IObservableBooleanInput"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IObservableBooleanInput Invert(this IObservableBooleanInput source)
        {
            return new ObserverableBooleanInputInverter(source);
        }

        public static void WaitFor(this IBooleanInput port, bool value)
        {
            while (port.Value != value)
            {
                Thread.Sleep(1);
            }
        }

        public static ScaleToRangeInput ScaleToRange(this IDoubleInput source, double minimum, double maximum)
        {
            return new ScaleToRangeInput(source, minimum, maximum);
        }

        public static SchmittTriggerInput SchmittTrigger(this IDoubleInput source, double triggerValue, double hysteresis)
        {
            return new SchmittTriggerInput(source, triggerValue, hysteresis);
        }
    }
}
