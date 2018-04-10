using System;

namespace AbstractIO
{
    public class ScaleToRangeInput : IDoubleInput
    {
        private IDoubleInput _source;
        private double _minimum, _maximum, _sourceMinimum, _sourceMaximum;

        public ScaleToRangeInput(IDoubleInput source, double minimum, double maximum)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (minimum >= maximum) { throw new ArgumentException(); }
            _source = source;
            _minimum = minimum;
            _maximum = maximum;
            _sourceMinimum = double.MaxValue;
            _sourceMaximum = double.MinValue;
        }

        public double Value
        {
            get
            {
                double sourceValue = _source.Value;

                if (sourceValue < _sourceMinimum) { _sourceMinimum = sourceValue; }
                if (sourceValue > _sourceMaximum) { _sourceMaximum = sourceValue; }

                double result;
                double interval = _sourceMaximum - _sourceMinimum;

                if (interval > 0.0)
                {
                    result = (sourceValue - _sourceMinimum) / interval * (_maximum - _minimum) + _minimum;
                }
                else
                {
                    result = (_maximum + _minimum) / 2.0;
                }
                if (result < _minimum)
                {
                    result = _minimum;
                }
                else if (result > _maximum)
                {
                    result = _maximum;
                }
                return result;
            }
        }
    }
}
