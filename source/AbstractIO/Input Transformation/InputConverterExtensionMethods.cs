namespace AbstractIO
{
    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language. Input transformation extension methods are named, by convention, using a
    /// verb such as "Invert".
    /// </summary>
    public static class InputConverterExtensionMethods
    {
        /// <summary>
        /// Creates a <see cref="BooleanInvertInput"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IBooleanInput Invert(this IBooleanInput source)
        {
            return new BooleanInvertInput(source);
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
            return new BooleanInvertObserverableInput(source);
        }

        /// <summary>
        /// Creates a <see cref="DoubleScaleToRangeInput"/> object scaling the values of this input to a specific numeric
        /// range.
        /// </summary>
        /// <param name="source">The source input to be scaled.</param>
        /// <param name="smallestValueMappedTo">The value that the smallest source input value will be mapped to.
        /// </param>
        /// <param name="largestValueMappedTo">The value that the largest source input value will be mapped to.</param>
        /// <remarks><paramref name="smallestValueMappedTo"/> may well be larger than
        /// <paramref name="largestValueMappedTo"/>. In this case the mapping will output a larger value for smaller
        /// source values, and you can, vor example, turn the sign of an input ranging from -1.0 to +1.0 into the
        /// resulting range +1.0 to -1.0. However, <paramref name="smallestValueMappedTo"/> must not be equal to
        /// <paramref name="largestValueMappedTo"/>.</remarks>
        /// <returns>The scaled input.</returns>
        public static DoubleScaleToRangeInput ScaleToRange(
            this IDoubleInput source,
            double smallestValueMappedTo,
            double largestValueMappedTo)
        {
            return new DoubleScaleToRangeInput(source, smallestValueMappedTo, largestValueMappedTo);
        }

        /// <summary>
        /// Creates a <see cref="SingleScaleToRangeInput"/> object scaling the values of this input to a specific numeric
        /// range.
        /// </summary>
        /// <param name="source">The source input to be scaled.</param>
        /// <param name="smallestValueMappedTo">The value that the smallest source input value will be mapped to.
        /// </param>
        /// <param name="largestValueMappedTo">The value that the largest source input value will be mapped to.</param>
        /// <remarks><paramref name="smallestValueMappedTo"/> may well be larger than
        /// <paramref name="largestValueMappedTo"/>. In this case the mapping will output a larger value for smaller
        /// source values, and you can, vor example, turn the sign of an input ranging from -1.0 to +1.0 into the
        /// resulting range +1.0 to -1.0. However, <paramref name="smallestValueMappedTo"/> must not be equal to
        /// <paramref name="largestValueMappedTo"/>.</remarks>
        /// <returns>The scaled input.</returns>
        public static SingleScaleToRangeInput ScaleToRange(
            this ISingleInput source,
            float smallestValueMappedTo,
            float largestValueMappedTo)
        {
            return new SingleScaleToRangeInput(source, smallestValueMappedTo, largestValueMappedTo);
        }

        /// <summary>
        /// Creates a <see cref="DoubleSchmittTriggerInput"/> object which maps a target <see cref="IDoubleInput"/> to a
        /// boolean using a threshold.
        /// </summary>
        /// <param name="source">The source input to be mapped from double to boolean.</param>
        /// <param name="threshold">The threshold above which <see cref="Value"/> shall return true, and below which
        /// it shall return false. This can by any number.</param>
        /// <param name="hysteresis">The range that the source value must be above/below the last value in order to
        /// change the resulting value. This must not be negative.</param>
        /// <returns>The mapped boolean input.</returns>
        /// <remarks>Example: Use 0.5 as <paramref name="threshold"/> and 0.1 as <paramref name="hysteresis"/>. The
        /// following sequence of source input values, in the given order, will then result in the following resulting
        /// values: 0.0 -> false, 1.0 -> true, 0.6 -> true, 0.5 -> true, 0.4 -> true, 0.3 -> false, 0.4 -> false,
        /// 0.5 -> false, 0.6 -> false, 0.7 -> true.</remarks>
        public static DoubleSchmittTriggerInput SchmittTrigger(
            this IDoubleInput source, 
            double threshold, 
            double hysteresis)
        {
            return new DoubleSchmittTriggerInput(source, threshold, hysteresis);
        }

        /// <summary>
        /// Creates a <see cref="SingleSchmittTriggerInput"/> object which maps a target <see cref="ISingleInput"/> to a
        /// boolean using a threshold.
        /// </summary>
        /// <param name="source">The source input to be mapped from float to boolean.</param>
        /// <param name="threshold">The threshold above which <see cref="Value"/> shall return true, and below which
        /// it shall return false. This can by any number.</param>
        /// <param name="hysteresis">The range that the source value must be above/below the last value in order to
        /// change the resulting value. This must not be negative.</param>
        /// <returns>The mapped boolean input.</returns>
        /// <remarks>Example: Use 0.5 as <paramref name="threshold"/> and 0.1 as <paramref name="hysteresis"/>. The
        /// following sequence of source input values, in the given order, will then result in the following resulting
        /// values: 0.0 -> false, 1.0 -> true, 0.6 -> true, 0.5 -> true, 0.4 -> true, 0.3 -> false, 0.4 -> false,
        /// 0.5 -> false, 0.6 -> false, 0.7 -> true.</remarks>
        public static SingleSchmittTriggerInput SchmittTrigger(
            this ISingleInput source,
            float threshold,
            float hysteresis)
        {
            return new SingleSchmittTriggerInput(source, threshold, hysteresis);
        }
    }
}
