namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IBooleanInput"/> combinding several other <see cref="IBooleanInput"/> objects using AND.
    /// </summary>
    public class BooleanAndInput : BooleanOperatorInputBase
    {
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInputs">The inputs to be operated on.</param>
        public BooleanAndInput(params IBooleanInput[] sourceInputs) : base(sourceInputs)
        {
        }

        /// <summary>
        /// Return true if all <see cref="SourceInputs"/> are true, otherwise false.
        /// </summary>
        public override bool Value
        {
            get
            {
                foreach (IBooleanInput input in SourceInputs)
                {
                    if (!input.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
