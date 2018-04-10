using System;

namespace AbstractIO
{
    /// <summary>
    /// A class mapping a boolean false/true to two double values.
    /// </summary>
    public class BooleanToDoubleMapper : IBooleanOutput
    {
        private IDoubleOutput _targetOutput;
        private bool _value;
        private double _falseValue, _trueValue;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targetOutput">The target output receiving the double values.</param>
        /// <param name="falseValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is false.</param>
        /// <param name="trueValue">The value that the <paramref name="targetOutput"/> shall be set to when the
        /// <see cref="IBooleanInput.Value"/> property is true.</param>
        public BooleanToDoubleMapper(IDoubleOutput targetOutput, double falseValue, double trueValue)
        {
            if (targetOutput == null)
            {
                throw new ArgumentNullException(nameof(targetOutput));
            }
            _targetOutput = targetOutput;
            _falseValue = falseValue;
            _trueValue = trueValue;
            _targetOutput.Value = falseValue;
        }

        /// <summary>
        /// Gets or sets the value of this object. When it is set to false/true, the target output object will be set to
        /// the values defined in the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
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
