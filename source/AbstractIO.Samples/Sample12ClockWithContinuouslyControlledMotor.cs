using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample12ClockWithContinuouslyControlledMotor
    {
        private class LinearEstimater
        {

            private struct Pair
            {
                public float X;
                public float Y;
            }

            private int _maxPairs;
            private Pair[] _pairs;
            private int _firstUsedIndex = 0;
            private int _nextIndexToWrite = 0;

            public LinearEstimater(int capacity)
            {
                if (capacity < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(capacity));
                }
                _maxPairs = capacity;
                _pairs = new Pair[_maxPairs];
            }

            public void Add(float x, float y)
            {
                _pairs[_nextIndexToWrite] = new Pair { X = x, Y = y };
                int newIndex = (_nextIndexToWrite + 1) % _maxPairs;
                if (newIndex == _firstUsedIndex)
                {
                    _firstUsedIndex = (_firstUsedIndex + 1) % _maxPairs;
                }
            }

            public int Count
            {
                get
                {
                    return (_nextIndexToWrite - _firstUsedIndex + _maxPairs) % _maxPairs;
                }
            }

            private void Calculate(ref float offset, ref float slope)
            {
                // Compute sums freshly (no cumulative errors on removing pairs):

                float sumX = 0f, sumY = 0f, sumX2 = 0f, sumY2 = 0f, sumXY = 0f;
                int index = _firstUsedIndex;
                while (index != _nextIndexToWrite)
                {
                    Pair p = _pairs[index];
                    float x = p.X;
                    float y = p.Y;
                    sumX += x;
                    sumY += y;
                    sumX2 += x * x;
                    sumY2 += y * y;
                    sumXY += x * y;
                    index = (index + 1) % _maxPairs;
                }

                // Compute regression:

                int n = Count;
                float b = n * sumXY - sumX * sumY;
                float a = n * sumX2 - sumX * sumX;
                float c = (n * sumY2 - sumY * sumY) * a;

                if (c < 0f)
                {
                    c = 0f;
                }

                offset = b / a;
                slope = (sumX2 * sumY - sumX * sumXY) / a;
            }

            public float EstimateY(float x)
            {
                float offset = 0f, slope = 0f;
                Calculate(ref offset, ref slope);
                return x * slope + offset;
            }

            public float EstimateX(float y)
            {
                float offset = 0f, slope = 0f;
                Calculate(ref offset, ref slope);
                return (y - offset) / slope;
            }
        }

        /// <summary>
        /// Runs a clock driven by a simple DC motor.
        /// </summary>
        /// <param name="motor">The motor to drive continuously.</param>
        /// <param name="pulse">The input which pulses to measure the motor speed.</param>
        /// <param name="millisecondsPerPulse">The number of milliseconds per pulse which would give a perfectly
        /// accurate operation of the clock.</param>
        /// <remarks>The motor speed is constantly adapted to the measurement given by the pulse to realize the needed
        /// pulse times without cumulative errors, even if the motor changes its behaviour during the operation.
        /// </remarks>
        public static void Run(ISingleOutput motor,
                               IBooleanInput pulse,
                               float millisecondsPerPulse,
                               IBooleanOutput secondsLamp)
        {
            // Start a seconds lamp:
            var secondsTimer = new Timer((state) => { secondsLamp.Value = !secondsLamp.Value; },
                                         null, 0, 500);

            var estimater = new LinearEstimater(10);

            Calibrate(motor, pulse, millisecondsPerPulse,estimater);

            // Run forever:
            Thread.Sleep(Timeout.Infinite);
        }

        private static void Calibrate(
            ISingleOutput motor, 
            IBooleanInput pulse, 
            float millisecondsPerPulse,
            LinearEstimater estimater)
        {
            // Let the motor run at full speed until the pulse changes from false to true:
            motor.Value = 1f;
            pulse.WaitFor(true, true);

            // Measure the time until pulse turns to true again:
            var start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var fastTime = DateTime.UtcNow - start;
            estimater.Add(1f, (float)fastTime.TotalMilliseconds);

            // Wait until shortly before the next pulse:
            Thread.Sleep((int)(fastTime.TotalMilliseconds / 10) * 9);

            // Let the motor run at 20% fullSpeedTime:
            motor.Value = 0.2f;
            pulse.WaitFor(true, true);

            start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var slowTime = DateTime.UtcNow - start;
            estimater.Add(0.2f, (float)slowTime.TotalMilliseconds);
        }
    }
}
