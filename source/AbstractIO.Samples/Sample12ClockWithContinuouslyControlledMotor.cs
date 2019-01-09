using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample12ClockWithContinuouslyControlledMotor
    {
        private class LinearEstimater
        {
            /// <summary>
            /// A pair of values for which linear regression will be calculated.
            /// </summary>
            private struct Pair
            {
                public float MotorSpeed;
                public float PulsesPerSecond;
            }

            /// <summary>
            /// The capacity of the estimater, that is, the maximum number of value pairs to keep.
            /// </summary>
            private int _capacity;

            /// <summary>
            /// The value pairs over which the statistical calculations will be done.
            /// </summary>
            private Pair[] _pairs;

            /// <summary>
            /// The valid number of pairs in <see cref="_pairs"/>.
            /// </summary>
            private int _count = 0;

            /// <summary>
            /// The index into <see cref="_pairs"/> into which the next value added by the
            /// <see cref="Add(float, float)"/> method will be stored.
            /// </summary>
            private int _nextIndexToWrite = 0;

            /// <summary>
            /// Creates an instance.
            /// </summary>
            /// <param name="capacity">The maximum number of value pairs to store. Only the last
            /// <paramref name="capacity"/> number of pairs are kept, older ones are dismissed.</param>
            public LinearEstimater(int capacity)
            {
                if (capacity < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(capacity));
                }
                _capacity = capacity;
                _pairs = new Pair[_capacity];
            }

            /// <summary>
            /// Adds a value pair to the list of pairs to be calculated over.
            /// </summary>
            /// <param name="motorSpeed">The motor speed for which a time measurement was done.</param>
            /// <param name="pulsesPerSecond">The measured number of pulses per second for the given
            /// <paramref name="motorSpeed"/>.</param>
            public void Add(float motorSpeed, float pulsesPerSecond)
            {
                _pairs[_nextIndexToWrite] =
                    new Pair
                    {
                        MotorSpeed = motorSpeed,
                        PulsesPerSecond = pulsesPerSecond
                    };

                if (_nextIndexToWrite + 1 > _count)
                {
                    _count = _nextIndexToWrite + 1;
                }

                _nextIndexToWrite = (_nextIndexToWrite + 1) % _capacity;

                Console.WriteLine("Added MotorSpeed = " + motorSpeed.ToString()
                                  + ", PulsesPerSecond = " + pulsesPerSecond.ToString());
            }

            /// <summary>
            /// Gets the number of value pairs stored.
            /// </summary>
            public int Count
            {
                get
                {
                    return _count;
                }
            }

            private void Calculate(ref float offset, ref float slope)
            {
                // Compute sums freshly (no cumulative errors on removing pairs):

                float sumX = 0f, sumY = 0f, sumX2 = 0f, sumY2 = 0f, sumXY = 0f;

                for (int index = 0; index < _count; index++)
                {
                    Pair p = _pairs[index];
                    float x = p.MotorSpeed;
                    float y = p.PulsesPerSecond;
                    sumX += x;
                    sumY += y;
                    sumX2 += x * x;
                    sumY2 += y * y;
                    sumXY += x * y;
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

                slope = b / a;
                offset = (sumX2 * sumY - sumX * sumXY) / a;

                Console.WriteLine("Calculate with count = " + n.ToString() + ": PulsesPerSecond = "
                                  + slope.ToString() + " * MotorSpeed "
                                  + (offset < 0f ? "- " : "+ ") + Math.Abs(offset).ToString());
            }

            public float EstimatePulsesPerSecond(float motorSpeed)
            {
                float offset = 0f, slope = 0f;
                Calculate(ref offset, ref slope);
                return motorSpeed * slope + offset;
            }

            public float EstimateMotorSpeed(float pulsesPerSecond)
            {
                float offset = 0f, slope = 0f;
                Calculate(ref offset, ref slope);
                return (pulsesPerSecond - offset) / slope;
            }
        }

        /// <summary>
        /// Runs a clock driven by a simple DC motor.
        /// </summary>
        /// <param name="motor">The motor to drive continuously.</param>
        /// <param name="pulse">The input which pulses to measure the motor speed.</param>
        /// <param name="pulsesPerSecond">The number of seconds per pulse which would give a perfectly
        /// accurate operation of the clock.</param>
        /// <remarks>The motor speed is constantly adapted to the measurement given by the pulse to realize the needed
        /// pulse times without cumulative errors, even if the motor changes its behaviour during the operation.
        /// </remarks>
        public static void Run(ISingleOutput motor,
                               IBooleanInput pulse,
                               float pulsesPerSecond,
                               IBooleanOutput secondsLamp)
        {
            // Start a blinking seconds lamp calmly going on in one second and off again in the next:
            var secondsTimer = new Timer((state) => { secondsLamp.Value = !secondsLamp.Value; },
                                         null, 0, 1000);

            var estimater = new LinearEstimater(64);

            CollectSpeedSamples(motor, pulse, estimater);

            // This is our starting point:
            var t0 = DateTime.UtcNow;

            while (true)
            {
                // This is the time we want the next gear cycle to end, ideally:
                var t1 = t0.AddSeconds(1f / pulsesPerSecond);

                // Calculate the needed motor speed to have this goal reached.
                // Note that we use the real "now" to calculate this, as we may have reached this point too early or
                // too late from the last cycle, and we aim to realize (at least in the long term) our ideally counted
                // time t.
                var speed = estimater.EstimateMotorSpeed(1f / (Single)(t1 - DateTime.UtcNow).TotalSeconds);

                if (speed < 0.1f)
                {
                    speed = 0.1f;
                }
                else if (speed > 1.0f)
                {
                    speed = 1.0f;
                }

                // Run the motor at this speed:
                motor.Value = speed;
                Console.WriteLine("Motor Speed = " + speed.ToString());

                // Wait for this cycle to end:
                pulse.WaitFor(true, true);

                // Take note of the new measurement:
                estimater.Add(speed, 1f / (float)(DateTime.UtcNow - t0).TotalSeconds);

                // t1 is the new t0:
                t0 = t1;
            }
        }

        /// <summary>
        /// Collects two speed samples to initialize the <see cref="LinearEstimater"/>.
        /// </summary>
        /// <param name="motor">The motor to drive.</param>
        /// <param name="pulse">The pulse switch.</param>
        /// <param name="estimater">The estimator to add the measured sample data to.</param>
        private static void CollectSpeedSamples(
            ISingleOutput motor,
            IBooleanInput pulse,
            LinearEstimater estimater)
        {
            // Let the motor run at full speed until the pulse changes from false to true:
            motor.Value = 1f;
            pulse.WaitFor(true, true);

            // Measure the time until pulse turns to true again:
            var start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var fastTime = DateTime.UtcNow - start;
            estimater.Add(1f, 1f / (float)fastTime.TotalSeconds);

            // Wait until shortly before the next pulse:
            Thread.Sleep((int)(fastTime.TotalMilliseconds / 10) * 9);

            // Let the motor run at 20% fullSpeedTime:
            motor.Value = 0.2f;
            pulse.WaitFor(true, true);

            start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var slowTime = DateTime.UtcNow - start;
            estimater.Add(0.2f, 1f / (float)slowTime.TotalSeconds);
        }
    }
}
