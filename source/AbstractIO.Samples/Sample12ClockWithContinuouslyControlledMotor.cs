using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample12ClockWithContinuouslyControlledMotor
    {
        /// <summary>
        /// The minimal speed that the DC motor shall receive.
        /// </summary>
        private const float MinimumMotorSpeed = 0.08f;

        private class LinearEstimater
        {
            /// <summary>
            /// The capacity of the estimater, that is, the maximum number of value pairs to keep.
            /// </summary>
            private const int Capacity = 128;

            /// <summary>
            /// A pair of values for which linear regression will be calculated.
            /// </summary>
            private struct Pair
            {
                public float MotorSpeed;
                public float PulsesPerSecond;
                public bool IsValid;
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
                        PulsesPerSecond = pulsesPerSecond,
                        IsValid = true
                    };

                Console.WriteLine("Added at index " + targetIndex.ToString()
                                  + ": MotorSpeed = " + motorSpeed.ToString()
                                  + ", PulsesPerSecond = " + pulsesPerSecond.ToString()
                                  + ", Time = " + (1f / pulsesPerSecond).ToString());
            }

            private void Calculate(ref float offset, ref float slope)
            {
                // Compute sums freshly (no cumulative errors on removing pairs):

                float sumX = 0f, sumY = 0f, sumX2 = 0f, sumY2 = 0f, sumXY = 0f;
                int count = 0;

                for (int index = 0; index < Capacity; index++)
                {
                    Pair pair = _pairs[index];
                    if (pair.IsValid)
                    {
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

            var estimater = new LinearEstimater();

            CollectSpeedSamples(motor, pulse, estimater);

            // This is our starting point:
            var startTime = DateTime.UtcNow;
            var t0 = startTime;
            int pulses = 0;

            while (true)
            {
                // This is the time we want the next gear cycle to end, ideally:
                pulses++;
                // Do not add some seconds every pulse but multiply and add to start time to avoid cumulative errors.
                var t1 = startTime.AddSeconds(pulses / pulsesPerSecond);

                // Calculate the needed motor speed to have this goal reached.
                // Note that we use the real "now" to calculate this, as we may have reached this point too early or
                // too late from the last cycle, and we aim to realize (at least in the long term) our ideally counted
                // time t.
                var speed = estimater.EstimateMotorSpeed(1f / (Single)(t1 - DateTime.UtcNow).TotalSeconds);

                if (speed < MinimumMotorSpeed)
                {
                    speed = MinimumMotorSpeed;
                }
                else if (speed > 1.0f)
                {
                    speed = 1.0f;
                }

                // Run the motor at this speed:
                motor.Value = speed;
                Console.WriteLine("Motor Speed = " + speed.ToString() + " at time " + DateTime.UtcNow.ToString());

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
