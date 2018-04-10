using System;
using System.Threading;

namespace AbstractIO
{
    #region Inverter

    /// <summary>
    /// A class inverting an <see cref="IBooleanOuput"/>.
    /// </summary>
    public class BooleanOutputInverter : IBooleanOutput
    {
        private readonly IBooleanOutput _target;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="target">The ouput get the inverted Value of this object.</param>
        public BooleanOutputInverter(IBooleanOutput target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Gets or sets the inverted Value of the target object passed to the constructor, or sets the target object's
        /// Value to the inverted Value of this object.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_target.Value;
            }
            set
            {
                _target.Value = !value;
            }
        }
    }

    #endregion

    #region Output Distribution

    /// <summary>
    /// A class distributing the values received to an arbitrary number of other <see cref="IBooleanOutput"/> objects.
    /// </summary>
    public class BooleanOutputDistributor : IBooleanOutput
    {
        private readonly IBooleanOutput[] _targets;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targets">The target objects whose <see cref="IBooleanOutput.Value"/> properties shall be set to
        /// the same value as the <see cref="Value"/> property of this object gets set.</param>
        public BooleanOutputDistributor(params IBooleanOutput[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                throw new ArgumentNullException(nameof(targets));
            }
            else
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null)
                    {
                        throw new ArgumentException("targets must not contain empty elements.");
                    }
                }
                _targets = new IBooleanOutput[targets.Length];
                targets.CopyTo(_targets, 0);
            }
        }

        /// <summary>
        /// Gets or sets the value. Setting it will set the values on all target objects passed to the constructor of
        /// this object.
        /// </summary>
        public bool Value
        {
            get
            {
                return _targets[0].Value;
            }
            set
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    _targets[i].Value = value;
                }
            }
        }
    }

    /// <summary>
    /// A class distributing the values received to an arbitrary number of other <see cref="IIntegerOutput"/> objects.
    /// </summary>
    public class IntegerOutputDistributor : IIntegerOutput
    {
        private readonly IIntegerOutput[] _targets;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targets">The target objects whose <see cref="IIntegerOutput.Value"/> properties shall be set to
        /// the same value as the <see cref="Value"/> property of this object gets set.</param>
        public IntegerOutputDistributor(params IIntegerOutput[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                throw new ArgumentNullException(nameof(targets));
            }
            else
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null)
                    {
                        throw new ArgumentException("targets must not contain empty elements.");
                    }
                }
                _targets = new IIntegerOutput[targets.Length];
                targets.CopyTo(_targets, 0);
            }
        }

        /// <summary>
        /// Gets or sets the value. Setting it will set the values on all target objects passed to the constructor of
        /// this object.
        /// </summary>
        public int Value
        {
            get
            {
                return _targets[0].Value;
            }
            set
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    _targets[i].Value = value;
                }
            }
        }
    }

    /// <summary>
    /// A class distributing the values received to an arbitrary number of other <see cref="IDoubleOutput"/> objects.
    /// </summary>
    public class DoubleOutputDistributor : IDoubleOutput
    {
        private readonly IDoubleOutput[] _targets;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targets">The target objects whose <see cref="IDoubleOutput.Value"/> properties shall be set to
        /// the same value as the <see cref="Value"/> property of this object gets set.</param>
        public DoubleOutputDistributor(params IDoubleOutput[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                throw new ArgumentNullException(nameof(targets));
            }
            else
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null)
                    {
                        throw new ArgumentException("targets must not contain empty elements.");
                    }
                }
                _targets = new IDoubleOutput[targets.Length];
                targets.CopyTo(_targets, 0);
            }
        }

        /// <summary>
        /// Gets or sets the value. Setting it will set the values on all target objects passed to the constructor of
        /// this object.
        /// </summary>
        public Double Value
        {
            get
            {
                return _targets[0].Value;
            }
            set
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    _targets[i].Value = value;
                }
            }
        }
    }

    #endregion

    public class BlinkWhenTrue : IBooleanOutput
    {
        private IBooleanOutput _targetOutput;
        private int _onDurationMs, _offDurationMs;
        private bool _currentState;
        private Thread _blinkThread;

        public BlinkWhenTrue(IBooleanOutput targetOutput, int onDurationMs, int offDurationMs)
        {
            if (targetOutput == null) { throw new ArgumentNullException(nameof(targetOutput)); }
            if (onDurationMs <= 0) { throw new ArgumentOutOfRangeException(nameof(onDurationMs)); }
            if (offDurationMs <= 0) { throw new ArgumentOutOfRangeException(nameof(offDurationMs)); }
            _targetOutput = targetOutput;
            _onDurationMs = onDurationMs;
            _offDurationMs = offDurationMs;
        }

        public bool Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (value != _currentState)
                {
                    if (value)
                    {
                        if (_blinkThread == null)
                        {
                            _blinkThread = new Thread(new ThreadStart(Blink));
                            _blinkThread.Start();
                        }
                        else
                        {
                            _blinkThread.Resume();
                        }
                    }
                    else
                    {
                        if (_blinkThread != null)
                        {
                            _blinkThread.Suspend();
                        }
                        _targetOutput.Value = false; ;
                    }
                    _currentState = value;
                }
            }
        }

        private void Blink()
        {
            while (true)
            {
                _targetOutput.Value = true;
                Thread.Sleep(_onDurationMs);
                _targetOutput.Value = false;
                Thread.Sleep(_offDurationMs);
            }
        }
    }

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

    public class OutputSmoother : IDoubleOutput
    {
        private IDoubleOutput _targetOutput;
        private double _targetValue, _currentValue, _rampStartValue;
        private TimeSpan _rampDuration, _rampStartTime;
        private int _stepPauseMs;
        private Thread _rampThread;

        public OutputSmoother(IDoubleOutput targetOutput, int rampTimeMs, int stepPauseMs)
        {
            if (targetOutput == null)
            {
                throw new ArgumentNullException(nameof(targetOutput));
            }
            if (rampTimeMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rampTimeMs));
            }
            if (stepPauseMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stepPauseMs));
            }
            _targetOutput = targetOutput;
            _rampDuration = TimeSpan.FromTicks(rampTimeMs * TimeSpan.TicksPerMillisecond);
            _stepPauseMs = stepPauseMs;
        }

        public double Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                _targetValue = value;
                if (_targetValue != _currentValue)
                {
                    throw new NotImplementedException();
                    //_rampStartTime = Microsoft.SPOT.Hardware.Utility.GetMachineTime();
                    _rampStartValue = _currentValue;
                    if (_rampThread == null)
                    {
                        _rampThread = new Thread(Ramp);
                        _rampThread.Start();
                    }
                    else if (_rampThread.ThreadState != ThreadState.Running)
                    {
                        _rampThread.Resume();
                    }
                }
            }
        }

        private void Ramp()
        {
            while (true)
            {
                if (_currentValue == _targetValue)
                {
                    _rampThread.Suspend();
                }
                else
                {
                    throw new NotImplementedException();
                    //var time = Microsoft.SPOT.Hardware.Utility.GetMachineTime();
                    //double nextValue;
                    //if (time >= _rampStartTime + _rampDuration)
                    //{
                    //    nextValue = _targetValue;
                    //}
                    //else
                    //{
                    //    nextValue = _rampStartValue +
                    //                (_targetValue - _rampStartValue) * (time - _rampStartTime).Ticks / //_rampDuration.Ticks;
                    //}
                    //_targetOutput.Write(nextValue);
                    //_currentValue = nextValue;
                    //Thread.Sleep(_stepPauseMs);
                }
            }
        }
    }

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