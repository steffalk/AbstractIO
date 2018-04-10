using System;

namespace AbstractIO
{
    public class SchmittTriggerInput : IBooleanInput
    {
        private IDoubleInput _sourceInput;
        private double _lowLimit, _highLimit;
        bool _currentValue;

        public SchmittTriggerInput(IDoubleInput sourceInput, double triggerValue, double hysteresis)
        {
            if (sourceInput == null) { throw new ArgumentNullException(nameof(sourceInput)); }
            if (hysteresis < 0.0) { throw new ArgumentOutOfRangeException(nameof(hysteresis)); }
            _sourceInput = sourceInput;
            hysteresis = hysteresis / 2.0;
            _lowLimit = triggerValue - hysteresis;
            _highLimit = triggerValue + hysteresis;
        }

        public bool Value
        {
            get
            {
                double value = _sourceInput.Value;

                if (value < _lowLimit)
                {
                    _currentValue = false;
                }
                else if (value > _highLimit)
                {
                    _currentValue = true;
                }
                return _currentValue;
            }
        }
    }

}
