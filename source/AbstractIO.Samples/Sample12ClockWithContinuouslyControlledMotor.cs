using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample12ClockWithContinuouslyControlledMotor
    {
        private class LinearEstimater
        {
            /// <summary>
            /// The capacity of the estimater, that is, the maximum number of value pairs to keep.
            /// </summary>
            private const int Capacity = 256;

            /// <summary>
            /// A pair of values for which linear regression will be calculated.
            /// </summary>
            private class Pair
            {
                public float MotorSpeed;
                public float PulsesPerSecond;
            }

            /// <summary>
            /// The value pairs over which the statistical calculations will be done.
            /// </summary>
            private Pair[] _pairs = new Pair[Capacity];

            /// <summary>
            /// Creates an instance.
            /// </summary>
            public LinearEstimater()
            {
            }

            /// <summary>
            /// Adds a value pair to the list of pairs to be calculated over.
            /// </summary>
            /// <param name="motorSpeed">The motor speed for which a time measurement was done.</param>
            /// <param name="pulsesPerSecond">The measured number of pulses per second for the given
            /// <paramref name="motorSpeed"/>.</param>
            public void Add(float motorSpeed, float pulsesPerSecond)
            {
                if (motorSpeed < 0f)
                {
                    motorSpeed = 0f;
                }
                else if (motorSpeed > 1f)
                {
                    motorSpeed = 1f;
                }
                int targetIndex = (int)Math.Round(motorSpeed * (Capacity - 1));

                _pairs[targetIndex] =
                    new Pair
                    {
                        MotorSpeed = motorSpeed,
                        PulsesPerSecond = pulsesPerSecond
                    };

                Console.WriteLine("Added at index " + targetIndex.ToString()
                                  + ": MotorSpeed = " + motorSpeed.ToString("N9")
                                  + ", PulsesPerSecond = " + pulsesPerSecond.ToString("N9")
                                  + ", Time = " + (1f / pulsesPerSecond).ToString("N9"));
            }

            private void Calculate(ref float offset, ref float slope)
            {
                // Compute sums freshly (no cumulative errors on removing pairs):

                float sumX = 0f, sumY = 0f, sumX2 = 0f, sumY2 = 0f, sumXY = 0f;
                int count = 0;

                for (int index = 0; index < Capacity; index++)
                {
                    Pair pair = _pairs[index];
                    if (pair != null)
                    {
                        // We have a measurement here.
                        float x = pair.MotorSpeed;
                        float y = pair.PulsesPerSecond;
                        sumX += x;
                        sumY += y;
                        sumX2 += x * x;
                        sumY2 += y * y;
                        sumXY += x * y;
                        count++;
                    }
                }

                // Compute regression:

                float b = count * sumXY - sumX * sumY;
                float a = count * sumX2 - sumX * sumX;
                float c = (count * sumY2 - sumY * sumY) * a;

                if (c < 0f)
                {
                    c = 0f;
                }

                slope = b / a;
                offset = (sumX2 * sumY - sumX * sumXY) / a;

                Console.WriteLine("Calculate with count = " + count.ToString() + ": PulsesPerSecond = "
                                  + slope.ToString("N9") + " * MotorSpeed "
                                  + (offset < 0f ? "- " : "+ ") + Math.Abs(offset).ToString("N9"));
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
        /// <param name="minimumMotorSpeed">The minimum speed setting that causes the motor to turn. Speeds below this
        /// threshold may cause the motor to not turn at all.</param>
        /// <param name="pulse">The input which pulses to measure the motor speed.</param>
        /// <param name="pulsesPerSecond">The number of seconds per pulse which would give a perfectly
        /// accurate operation of the clock.</param>
        /// <param name="secondsLamp">A lamp which shall be turned on and off in 1 second intervals.</param>
        /// <remarks>The motor speed is constantly adapted to the measurement given by the pulse to realize the needed
        /// pulse times without cumulative errors, even if the motor changes its behaviour during the operation.
        /// </remarks>
        public static void Run(ISingleOutput motor,
                               float minimumMotorSpeed,
                               IBooleanInput pulse,
                               float pulsesPerSecond,
                               IBooleanOutput secondsLamp)
        {
            // Start a blinking seconds lamp calmly going on in one second and off again in the next:
            var secondsTimer = new Timer((state) => { secondsLamp.Value = !secondsLamp.Value; },
                                         null, 0, 1000);

            var estimater = new LinearEstimater();

            CollectSpeedSamples(motor, pulse, estimater);

            // This is our starting point:
            var startTime = DateTime.UtcNow;
            var beginOfCurrentPeriod = startTime;
            int pulses = 0;
            var idealSecondsForPulse = 1f / pulsesPerSecond;

            while (true)
            {
                // Do not add some seconds every pulse but multiply and add to start time to avoid cumulative errors.
                // This is the time we want the next gear cycle to end, ideally:
                pulses++;

                double deviation = (DateTime.UtcNow - beginOfCurrentPeriod).TotalSeconds;
                if (deviation < 0.0)
                {
                    Console.WriteLine("Too fast by " + (-deviation).ToString("N9") + " s");
                }
                else if (deviation > 0.0)
                {
                    Console.WriteLine("Too slow by " + deviation.ToString("N9") + " s");
                }

                var beginOfNextPeriod = startTime.AddSeconds(pulses / pulsesPerSecond);

                // Calculate the needed motor speed to have this goal reached.
                // Note that we use the real "now" to calculate this, as we may have reached this point too early or
                // too late from the last cycle, and we aim to realize (at least in the long term) our ideally counted
                // time t.
                var speed = Math.Max(
                                Math.Min(
                                    estimater.EstimateMotorSpeed(1f / (Single)(beginOfNextPeriod - DateTime.UtcNow)
                                                                              .TotalSeconds),
                                    1f),
                                minimumMotorSpeed);

                // Run the motor at this speed:
                motor.Value = speed;
                Console.WriteLine("Motor Speed = " + speed.ToString("N9")
                                  + " at time " + DateTime.UtcNow.ToString()
                                  + "; deviation = "
                                  + (DateTime.UtcNow - beginOfCurrentPeriod).TotalSeconds.ToString("N9"));

                bool repeat;
                do
                {
                    // Wait for this cycle to end:
                    pulse.WaitFor(true, true);

                    // Take note of the new measurement:
                    var secondsForThisPulse = (float)(DateTime.UtcNow - beginOfCurrentPeriod).TotalSeconds;

                    // Only use the measurement if it is withing 20% of the ideal time, otherwise assume that the it
                    // failed due to multiple or missing pulses:
                    if (Math.Abs(secondsForThisPulse - idealSecondsForPulse) / idealSecondsForPulse < 0.2f)
                    {
                        // The measurement is in the expected accuracy range. Take it into account for future
                        // calculations:
                        estimater.Add(speed, 1f / secondsForThisPulse);
                        repeat = false;
                    }
                    else if (secondsForThisPulse > idealSecondsForPulse)
                    {
                        // The detector probably missed one or more pulses. Adapt the time at which the clock is:
                        pulses += (int)(Math.Round(secondsForThisPulse / idealSecondsForPulse)) - 1;
                        beginOfNextPeriod = startTime.AddSeconds(pulses / pulsesPerSecond);
                        repeat = false;
                    }
                    else
                    {
                        // We had multiple switch pulses in this period. Wait for the next pulse:
                        repeat = true;
                    }
                } while (repeat);


                // Prepare for the next period:
                beginOfCurrentPeriod = beginOfNextPeriod;
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

            const float slowSpeed = 0.2f;

            motor.Value = slowSpeed;
            pulse.WaitFor(true, true);

            start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var slowTime = DateTime.UtcNow - start;
            estimater.Add(slowSpeed, 1f / (float)slowTime.TotalSeconds);
        }
    }
}
