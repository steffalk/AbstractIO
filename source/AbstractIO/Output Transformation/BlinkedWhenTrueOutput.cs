using System;
using System.Threading;

namespace AbstractIO
{
    /// <summary>
    /// An <see cref="IBooleanOutput"/> which will blink when and as long as a source <see cref="IBooleanOutput"/> is
    /// true.
    /// </summary>
    public class BlinkedWhenTrueOutput : IBooleanOutput
    {
        private IBooleanOutput _targetOutput;
        private int _onDurationMs, _offDurationMs;
        private bool _currentValue;
        private Thread _blinkThread;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targetOutput">The output which shall "blink", that is, periodically turned to true and false,
        /// when and as long as the <see cref="Value"/> property is true.</param>
        /// <param name="onDurationMs">The number of milliseconds for the true-phase of the blinker.</param>
        /// <param name="offDurationMs">The number of milliseconds for the false-phase of the blinker.</param>
        public BlinkedWhenTrueOutput(IBooleanOutput targetOutput, int onDurationMs, int offDurationMs)
        {
            _targetOutput = targetOutput ?? throw new ArgumentNullException(nameof(targetOutput));
            if (onDurationMs <= 0) { throw new ArgumentOutOfRangeException(nameof(onDurationMs)); }
            if (offDurationMs <= 0) { throw new ArgumentOutOfRangeException(nameof(offDurationMs)); }

            _onDurationMs = onDurationMs;
            _offDurationMs = offDurationMs;
        }

        /// <summary>
        /// Gets or sets whether the output shall blink.
        /// </summary>
        public bool Value
        {
            get
            {
                return _currentValue;
            }
            set
            {
                if (value != _currentValue)
                {
                    if (value)
                    {
                        if (_blinkThread == null)
                        {
                            _blinkThread = new Thread(Blink);
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
                        _targetOutput.Value = false;
                    }
                    _currentValue = value;
                }
            }
        }

        /// <summary>
        /// Actually blinks the output. This method will run on its own thread.
        /// </summary>
        private void Blink()
        {
            while (true)
            {
                _targetOutput.Value = true;
                Thread.Sleep(_onDurationMs);

                // Do not turn the "lamp" on if it was turned off during the above Sleep. This will happen when the
                // thread got suspended and then resumed. In this case, we want to start with turning the lamp on
                // as soon as Value is set to true again and not go through the remaining "off" period from the last
                // blink cycle.
                if (_targetOutput.Value)
                {
                    _targetOutput.Value = false;
                    Thread.Sleep(_offDurationMs);
                }
            }
        }
    }
}
