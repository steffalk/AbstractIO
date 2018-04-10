using System;
using System.Threading;

namespace AbstractIO
{
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
}
