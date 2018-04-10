namespace AbstractIO
{
    public static class OutputConverterExtensionMethods
    {
        /// <summary>
        /// Creates an <see cref="BooleanOutputInverter"/> object.
        /// </summary>
        /// <param name="target">The output which shall receive the inverted Value of this object.</param>
        /// <returns>The <see cref="BooleanOutputInverter"/> object sending inverted
        /// <see cref="IBooleanOutput">Values</see> to <paramref name="target"/>.</returns>
        public static IBooleanOutput Invert(this IBooleanOutput target)
        {
            return new BooleanOutputInverter(target);
        }
        public static BlinkWhenTrue BlinkWhenTrue(this IBooleanOutput targetOutput, int onDurationMs, int offDurationMs)
        {
            return new BlinkWhenTrue(targetOutput, onDurationMs, offDurationMs);
        }

        public static BooleanToDoubleMapper MapBooleanToDouble(this IDoubleOutput targetOutput,
                                                               double falseValue,
                                                               double trueValue)
        {
            return new BooleanToDoubleMapper(targetOutput, falseValue, trueValue);
        }

        public static OutputSmoother SmoothOutput(this IDoubleOutput targetOutput, int rampTimeMs, int stepPauseMs)
        {
            return new OutputSmoother(targetOutput, rampTimeMs, stepPauseMs);
        }
    }
}