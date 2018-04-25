namespace AbstractIO
{
    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language. Output transformation extension methods are named, by convention, using an
    /// adjective such as "Inverted".
    /// </summary>
    public static class OutputConverterExtensionMethods
    {
        /// <summary>
        /// Creates an <see cref="InvertedBooleanOutput"/> object.
        /// </summary>
        /// <param name="target">The output which shall receive the inverted Value of this object.</param>
        /// <returns>The <see cref="InvertedBooleanOutput"/> object sending inverted
        /// <see cref="IBooleanOutput">Values</see> to <paramref name="target"/>.</returns>
        public static IBooleanOutput Inverted(this IBooleanOutput target)
        {
            return new InvertedBooleanOutput(target);
        }

        /// <summary>
        /// Creates an <see cref="BlinkedWhenTrueOutput"/> object letting an boolean target output "blink" when and as
        /// // long as the input value is true.
        /// </summary>
        /// <param name="targetOutput">The output which shall "blink", that is, periodically turned to true and false,
        /// when and as long as the <see cref="Value"/> property is true.</param>
        /// <param name="onDurationMs">The number of milliseconds for the true-phase of the blinker.</param>
        /// <param name="offDurationMs">The number of milliseconds for the false-phase of the blinker.</param>
        /// <returns>The created input which will blink the <paramref name="targetOutput"/> when and as long as its
        /// <see cref="IBooleanInput.Value"/> property is true.</returns>
        public static BlinkedWhenTrueOutput BlinkedWhenTrue(
            this IBooleanOutput targetOutput,
            int onDurationMs,
            int offDurationMs)
        {
            return new BlinkedWhenTrueOutput(targetOutput, onDurationMs, offDurationMs);
        }

        /// <summary>
        /// Creates a <see cref="MappedFromBooleanOutput"/> object mapping the boolean values false/true to two double
        /// values.
        /// </summary>
        /// <param name="targetOutput">The target output receiving the double values.</param>
        /// <param name="falseValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is false.</param>
        /// <param name="trueValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is true.</param>
        /// <returns></returns>
        public static MappedFromBooleanOutput MappedFromBoolean(
            this IDoubleOutput targetOutput,
            double falseValue,
            double trueValue)
        {
            return new MappedFromBooleanOutput(targetOutput, falseValue, trueValue);
        }

        /// <summary>
        /// Creates a <see cref="SmoothedOutput"/> object which will slowly approach the
        /// <paramref name="targetOutput"/> value to the goal <see cref="IDoubleOutput.Value"/>.
        /// </summary>
        /// <param name="targetOutput">The target output to be smoothed.</param>
        /// <param name="valueChangePerSecond">The amount by that the <paramref name="targetOutput"/> value shall change
        /// per second in order to reach the <see cref="Value"/> property which definies the goal value.</param>
        /// <param name="rampIntervalMs">The interval, in milliseconds, in which the <paramref name="targetOutput"/>
        /// value shall be computed and set. The smaller this value, the more often and more smoothly will the target
        /// value be adapted.</param>
        /// <returns>The created <see cref="SmoothedOutput"/> object.</returns>
        public static SmoothedOutput Smoothed(
            this IDoubleOutput targetOutput,
            double valueChangePerSecond,
            int rampIntervalMs)
        {
            return new SmoothedOutput(targetOutput, valueChangePerSecond, rampIntervalMs);
        }

        /// <summary>
        /// Creates a <see cref="ScaledOutput"/> object which will scale values linear using a factor and an offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="factor">The factor to use.</param>
        /// <param name="offset">The offset to use.</param>
        /// <returns>The creted <see cref="ScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="IDoubleOutput.Value">Value</see> * <paramref name="factor"/> + <paramref name="offset"/>.
        /// </remarks>
        public static ScaledOutput Scaled(this IDoubleOutput targetOutput, double factor, double offset)
        {
            return new ScaledOutput(targetOutput, factor, offset);
        }

        /// <summary>
        /// Creates a <see cref="ScaledOutput"/> object which will scale values linear using a factor only and 0.0 as
        /// the offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="factor">The factor to use.</param>
        /// <returns>The creted <see cref="ScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="IDoubleOutput.Value">Value</see> * <paramref name="factor"/>.</remarks>
        public static ScaledOutput Scaled(this IDoubleOutput targetOutput, double factor)
        {
            return new ScaledOutput(targetOutput, factor);
        }
    }
}