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
        /// Creates an <see cref="BooleanInvertedOutput"/> object.
        /// </summary>
        /// <param name="target">The output which shall receive the inverted Value of this object.</param>
        /// <returns>The <see cref="BooleanInvertedOutput"/> object sending inverted
        /// <see cref="IBooleanOutput">Values</see> to <paramref name="target"/>.</returns>
        public static IBooleanOutput Inverted(this IBooleanOutput target)
        {
            return new BooleanInvertedOutput(target);
        }

        /// <summary>
        /// Creates an <see cref="BooleanBlinkedWhenTrueOutput"/> object letting an boolean target output "blink" when and as
        /// // long as the input value is true.
        /// </summary>
        /// <param name="targetOutput">The output which shall "blink", that is, periodically turned to true and false,
        /// when and as long as the <see cref="Value"/> property is true.</param>
        /// <param name="onDurationMs">The number of milliseconds for the true-phase of the blinker.</param>
        /// <param name="offDurationMs">The number of milliseconds for the false-phase of the blinker.</param>
        /// <returns>The created input which will blink the <paramref name="targetOutput"/> when and as long as its
        /// <see cref="IBooleanInput.Value"/> property is true.</returns>
        public static BooleanBlinkedWhenTrueOutput BlinkedWhenTrue(
            this IBooleanOutput targetOutput,
            int onDurationMs,
            int offDurationMs)
        {
            return new BooleanBlinkedWhenTrueOutput(targetOutput, onDurationMs, offDurationMs);
        }

        /// <summary>
        /// Creates a <see cref="DoubleMappedFromBooleanOutput"/> object mapping the boolean values false/true to two
        /// double values.
        /// </summary>
        /// <param name="targetOutput">The target output receiving the double values.</param>
        /// <param name="falseValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is false.</param>
        /// <param name="trueValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is true.</param>
        /// <returns></returns>
        public static DoubleMappedFromBooleanOutput MappedFromBoolean(
            this IDoubleOutput targetOutput,
            double falseValue,
            double trueValue)
        {
            return new DoubleMappedFromBooleanOutput(targetOutput, falseValue, trueValue);
        }

        /// <summary>
        /// Creates a <see cref="SingleMappedFromBooleanOutput"/> object mapping the boolean values false/true to two
        /// float values.
        /// </summary>
        /// <param name="targetOutput">The target output receiving the float values.</param>
        /// <param name="falseValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is false.</param>
        /// <param name="trueValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is true.</param>
        /// <returns></returns>
        public static SingleMappedFromBooleanOutput MappedFromBoolean(
            this ISingleOutput targetOutput,
            float falseValue,
            float trueValue)
        {
            return new SingleMappedFromBooleanOutput(targetOutput, falseValue, trueValue);
        }

        /// <summary>
        /// Creates a <see cref="DoubleSmoothedOutput"/> object which will slowly approach the
        /// <paramref name="targetOutput"/> value to the goal <see cref="IDoubleOutput.Value"/>.
        /// </summary>
        /// <param name="targetOutput">The target output to be smoothed.</param>
        /// <param name="valueChangePerSecond">The amount by that the <paramref name="targetOutput"/> value shall change
        /// per second in order to reach the <see cref="Value"/> property which definies the goal value.</param>
        /// <param name="rampIntervalMs">The interval, in milliseconds, in which the <paramref name="targetOutput"/>
        /// value shall be computed and set. The smaller this value, the more often and more smoothly will the target
        /// value be adapted.</param>
        /// <returns>The created <see cref="DoubleSmoothedOutput"/> object.</returns>
        public static DoubleSmoothedOutput Smoothed(
            this IDoubleOutput targetOutput,
            double valueChangePerSecond,
            int rampIntervalMs)
        {
            return new DoubleSmoothedOutput(targetOutput, valueChangePerSecond, rampIntervalMs);
        }

        /// <summary>
        /// Creates a <see cref="SingleSmoothedOutput"/> object which will slowly approach the
        /// <paramref name="targetOutput"/> value to the goal <see cref="ISingleOutput.Value"/>.
        /// </summary>
        /// <param name="targetOutput">The target output to be smoothed.</param>
        /// <param name="valueChangePerSecond">The amount by that the <paramref name="targetOutput"/> value shall change
        /// per second in order to reach the <see cref="Value"/> property which definies the goal value.</param>
        /// <param name="rampIntervalMs">The interval, in milliseconds, in which the <paramref name="targetOutput"/>
        /// value shall be computed and set. The smaller this value, the more often and more smoothly will the target
        /// value be adapted.</param>
        /// <returns>The created <see cref="SingleSmoothedOutput"/> object.</returns>
        public static SingleSmoothedOutput Smoothed(
            this ISingleOutput targetOutput,
            float valueChangePerSecond,
            int rampIntervalMs)
        {
            return new SingleSmoothedOutput(targetOutput, valueChangePerSecond, rampIntervalMs);
        }

        /// <summary>
        /// Creates a <see cref="DoubleScaledOutput"/> object which will scale values quadratic and linear using a
        /// factor and an offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="quadraticCoefficient">The factor by which the square of the value will be used.</param>
        /// <param name="factor">The factor to use.</param>
        /// <param name="offset">The offset to use.</param>
        /// <returns>The created <see cref="DoubleScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="IDoubleOutput.Value">Value</see> * <paramref name="factor"/> + <paramref name="offset"/>.
        /// </remarks>
        public static DoubleScaledOutput Scaled(
            this IDoubleOutput targetOutput,
            double quadraticCoefficient,
            double factor,
            double offset)
        {
            return new DoubleScaledOutput(targetOutput, quadraticCoefficient, factor, offset);
        }

        /// <summary>
        /// Creates a <see cref="DoubleScaledOutput"/> object which will scale values linear using a factor and an
        /// offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="factor">The factor to use.</param>
        /// <param name="offset">The offset to use.</param>
        /// <returns>The created <see cref="DoubleScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="IDoubleOutput.Value">Value</see> * <paramref name="factor"/> + <paramref name="offset"/>.
        /// </remarks>
        public static DoubleScaledOutput Scaled(this IDoubleOutput targetOutput, double factor, double offset)
        {
            return new DoubleScaledOutput(targetOutput, factor, offset);
        }

        /// <summary>
        /// Creates a <see cref="DoubleScaledOutput"/> object which will scale values linear using a factor only and
        /// 0.0 as the offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="factor">The factor to use.</param>
        /// <returns>The created <see cref="DoubleScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="IDoubleOutput.Value">Value</see> * <paramref name="factor"/>.</remarks>
        public static DoubleScaledOutput Scaled(this IDoubleOutput targetOutput, double factor)
        {
            return new DoubleScaledOutput(targetOutput, factor);
        }

        /// <summary>
        /// Creates a <see cref="SingleScaledOutput"/> object which will scale values linear using a factor and an
        /// offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="quadraticCoefficient">The factor by which the square of the value will be used.</param>
        /// <param name="factor">The factor to use.</param>
        /// <param name="offset">The offset to use.</param>
        /// <returns>The created <see cref="SingleScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="ISingleOutput.Value">Value</see> * <paramref name="factor"/> + <paramref name="offset"/>.
        /// </remarks>
        public static SingleScaledOutput Scaled(
            this ISingleOutput targetOutput,
            float quadraticCoefficient,
            float factor,
            float offset)
        {
            return new SingleScaledOutput(targetOutput, quadraticCoefficient, factor, offset);
        }

        /// <summary>
        /// Creates a <see cref="SingleScaledOutput"/> object which will scale values linear using a factor and an
        /// offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="factor">The factor to use.</param>
        /// <param name="offset">The offset to use.</param>
        /// <returns>The created <see cref="SingleScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="ISingleOutput.Value">Value</see> * <paramref name="factor"/> + <paramref name="offset"/>.
        /// </remarks>
        public static SingleScaledOutput Scaled(this ISingleOutput targetOutput, float factor, float offset)
        {
            return new SingleScaledOutput(targetOutput, factor, offset);
        }

        /// <summary>
        /// Creates a <see cref="SingleScaledOutput"/> object which will scale values linear using a factor only and
        /// 0.0 as the offset.
        /// </summary>
        /// <param name="targetOutput">The target output to received the scaled values.</param>
        /// <param name="factor">The factor to use.</param>
        /// <returns>The created <see cref="SingleScaledOutput"/> object.</returns>
        /// <remarks>The <paramref name="targetOutput"/> will receive values scaled by the formula:
        /// <see cref="ISingleOutput.Value">Value</see> * <paramref name="factor"/>.</remarks>
        public static SingleScaledOutput Scaled(this ISingleOutput targetOutput, float factor)
        {
            return new SingleScaledOutput(targetOutput, factor);
        }

        /// <summary>
        /// Distributes an <see cref="IBooleanOutput"/> to another one in copy.
        /// </summary>
        /// <param name="targetOutput">The output whose value shall be passed to another output whenever it is set.
        /// </param>
        /// <param name="monitor">The other output, which shall receive whatever <paramref name="targetOutput"/>
        /// receives.</param>
        /// <returns>The created <see cref="BooleanOutputDistributor"/> object.</returns>
        /// <remarks>If you need multiple monitors, directly use the <see cref="BooleanOutputDistributor"/> class whose
        /// constructor accepts any number of <see cref="IBooleanOutput"/> objects.</remarks>
        public static BooleanOutputDistributor Distributed(this IBooleanOutput targetOutput, IBooleanOutput monitor)
        {
            return new BooleanOutputDistributor(targetOutput, monitor);
        }
    }
}