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
    public class DoubleSmoothedOutput : IDoubleOutput, ITargetReachedObservable
    {
        // The target output which shall received the smoothly accelerated/decelerated output:
        private IDoubleOutput _targetOutput;

        // The acceleration/deceleration in units of change per tick:
        private double _valueChangePerTick;

        // The interval, in ms, at which the target output value shall be adapted when accelerating/decelearting:
        private int _rampIntervalMs;

        // The target value up to which the target output shall be accelerated/decelerated:
        private double _targetValue;

        // The timer used to periodically update the target output value:
        private Timer _timer;

        // The UTC time ticks when acceleration started:
        private long _startTimeTicks;

        // The value at the time the acceleration started:
        private double _startValue;

        // The signed acceleration as a value change per tick to be used for the current acceleration/deceleration:
        private double _signedValuePerTick;

        // This input will be set (and thus raise events) when the _targetValue is reached.
        private BooleanSettableInput _isTargetReached = new BooleanSettableInput(true);

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targetOutput">The target output to be smoothed.</param>
        /// <param name="valueChangePerSecond">The amount by that the <paramref name="targetOutput"/> value shall change
        /// per second in order to reach the <see cref="Value"/> property which definies the goal value.</param>
        /// <param name="rampIntervalMs">The interval, in milliseconds, in which the <paramref name="targetOutput"/>
        /// value shall be computed and set. The smaller this value, the more often and more smoothly will the target
        /// value be adapted.</param>
        public DoubleSmoothedOutput(IDoubleOutput targetOutput, double valueChangePerSecond, int rampIntervalMs)
        {
            _targetOutput = targetOutput ?? throw new ArgumentNullException(nameof(targetOutput));
            if (valueChangePerSecond <= 0.0) throw new ArgumentOutOfRangeException(nameof(valueChangePerSecond));
            if (rampIntervalMs <= 0) throw new ArgumentOutOfRangeException(nameof(rampIntervalMs));

            _valueChangePerTick = valueChangePerSecond / TimeSpan.TicksPerSecond;
            _rampIntervalMs = rampIntervalMs;
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
                _startValue = _targetOutput.Value;
                if (_targetValue == _startValue)
                {
                    _isTargetReached.Value = true;
                }
                else
                {
                    _isTargetReached.Value = false;
                    // Remember when and at what actual current value the accleration started:
                    _startTimeTicks = DateTime.UtcNow.Ticks;
                    if (_targetValue > _startValue)
                    {
                        _signedValuePerTick = _valueChangePerTick;
                    }
                    else
                    {
                        _signedValuePerTick = -_valueChangePerTick;
                    }

                    // Create or resume the acceleration timer:
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
        /// Gets an <see cref="IObservableBooleanInput"/> whose <see cref="IBooleanInput.Value">Value</see> property
        /// returns true when a set target <see cref="Value"/> value has actually been reached by the smoothed output.
        /// </summary>
        /// <remarks>
        /// As the returned object will also raise an event when this state changes, so you can easily wait for the
        /// smoothed output to reach the desired target value.
        /// </remarks>
        public IObservableBooleanInput IsTargetReached
        {
            get
            {
                return _isTargetReached;
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
                _isTargetReached.Value = true;
            }
            else
            {
                // New value = [Start Value] + [Time Difference] * [Acceleration], aka "v = v0 + a * t":
                double newValue = _startValue + (DateTime.UtcNow.Ticks - _startTimeTicks) * _signedValuePerTick;

                if (_signedValuePerTick > 0)
                {
                    if (newValue > _targetValue)
                    {
                        newValue = _targetValue;
                    }
                }
                else
                {
                    if (newValue < _targetValue)
                    {
                        newValue = _targetValue;
                    }
                }

                currentValue = newValue;
                _targetOutput.Value = newValue;
                if (currentValue == _targetValue)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _isTargetReached.Value = true;
                }
            }
        }
    }
}
