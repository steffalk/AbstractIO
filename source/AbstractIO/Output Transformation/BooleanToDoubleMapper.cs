using System;

namespace AbstractIO
{
    public class BooleanToDoubleMapper : IBooleanOutput
    {
        private IDoubleOutput _targetOutput;
        double _falseValue, _trueValue;

        public BooleanToDoubleMapper(IDoubleOutput targetOutput, double falseValue, double trueValue)
        {
            if (targetOutput == null)
            {
                throw new ArgumentNullException(nameof(targetOutput));
            }
            _targetOutput = targetOutput;
            _falseValue = falseValue;
            _trueValue = trueValue;
        }

        public bool Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (value)
                {
                    _targetOutput.Value = _trueValue;
                }
                else
                {
                    _targetOutput.Value = _falseValue;
                }
            }
        }
    }
}
