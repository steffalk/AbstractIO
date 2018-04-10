namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IBooleanInput"/> combinding several other <see cref="IBooleanInput"/> objects using OR.
    /// </summary>
    public class BooleanOrInput : BooleanOperatorInputBase
    {
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInputs">The inputs to be operated on.</param>
        public BooleanOrInput(params IBooleanInput[] sourceInputs) : base(sourceInputs)
        {
        }

        /// <summary>
        /// Return true if at least one of the <see cref="SourceInputs"/> is true, otherwise false.
        /// </summary>
        public override bool Value
        {
            get
            {
                foreach (IBooleanInput input in SourceInputs)
                {
                    if (input.Value)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

}
