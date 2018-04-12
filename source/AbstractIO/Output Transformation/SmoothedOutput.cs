using System;
using System.Threading;

namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IDoubleOutput"/> which lets a target <see cref="IDoubleOutput"/> smoothly approach a target value.
    /// </summary>
    /// <remarks>
    /// For example, if you want to have a light turning on and offsmoothly, you can use this class to map the values
    /// 0.0 (lamp off) and 1.0 (lamp fully on) to a smooth ramp of slowly enlighting the lamp from 0.0 slowlow to 1.0
    /// // and dimming the lamp slowly back from 1.0 to 0.0.
    /// </remarks>
    public class SmoothedOutput : IDoubleOutput
    {
        private IDoubleOutput _targetOutput;
        private double _targetValue, _valueChangePerSecond;
        private int _rampIntervalMs;
        private Timer _timer;
        private double _changePerInterval;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targetOutput">The target output to be smoothed.</param>
        /// <param name="valueChangePerSecond">The amount by that the <paramref name="targetOutput"/> value shall change
        /// per second in order to reach the <see cref="Value"/> property which definies the goal value.</param>
        /// <param name="rampIntervalMs">The interval, in milliseconds, in which the <paramref name="targetOutput"/>
        /// value shall be computed and set. The smaller this value, the more often and more smoothly will the target
        /// value be adapted.</param>
        public SmoothedOutput(IDoubleOutput targetOutput, double valueChangePerSecond, int rampIntervalMs)
        {
            _targetOutput = targetOutput ?? throw new ArgumentNullException(nameof(targetOutput));
            if (valueChangePerSecond <= 0.0) { throw new ArgumentOutOfRangeException(nameof(valueChangePerSecond)); }
            if (rampIntervalMs <= 0) { throw new ArgumentOutOfRangeException(nameof(rampIntervalMs)); }

            _valueChangePerSecond = valueChangePerSecond;
            _rampIntervalMs = rampIntervalMs;
            _changePerInterval = valueChangePerSecond * rampIntervalMs / 1000.0;
        }

        /// <summary>
        /// Gets or sets the value to which the target output shall approach.
        /// </summary>
        /// <remarks>
        /// The inital value is 0.0.
        /// </remarks>
        public double Value
        {
            get
            {
                return _targetValue;
            }
            set
            {
                _targetValue = value;
                if (_targetValue != _targetOutput.Value)
                {
                    if (_timer == null)
                    {
                        _timer = new Timer(ChangeTargetValue, null, 0, _rampIntervalMs);
                    }
                    else
                    {
                        _timer.Change(0, _rampIntervalMs);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the target value. This method will run on the used Timer's thread.
        /// </summary>
        /// <param name="ignoredState"></param>
        private void ChangeTargetValue(object ignoredState)
        {
            double currentValue = _targetOutput.Value;

            if (currentValue == _targetValue)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            else
            {
                double newValue;

                if (currentValue < _targetValue)
                {
                    newValue = Math.Min(currentValue + _changePerInterval, _targetValue);
                }
                else
                {
                    newValue = Math.Max(currentValue - _changePerInterval, _targetValue);
                }
                currentValue = newValue;
                _targetOutput.Value = newValue;
            }
        }
    }
}
