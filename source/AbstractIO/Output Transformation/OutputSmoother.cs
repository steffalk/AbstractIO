using System;
using System.Threading;

namespace AbstractIO
{
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
}
