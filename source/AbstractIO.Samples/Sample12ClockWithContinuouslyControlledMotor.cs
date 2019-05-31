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
        /// <param name="pulseDebounceMillisecondsAtFullSpeed">The time, in milliseconds, that shall be used as the
        /// debounce time for the <paramref name="pulse"/> input when the <paramref name="motor"/> runs at full speed.
        /// </param>
        /// <param name="pulseMonitor">An output to show the monitored pulse input.</param>
        /// <param name="pulsesPerSecond">The number of seconds per pulse which would give a perfectly
        /// accurate operation of the clock.</param>
        /// <param name="secondsLamp">A lamp which shall be turned on and off in 1 second intervals.</param>
        /// <remarks>The motor speed is constantly adapted to the measurement given by the pulse to realize the needed
        /// pulse times without cumulative errors, even if the motor changes its behaviour during the operation.
        /// </remarks>
        public static void Run(ISingleOutput motor,
                               float minimumMotorSpeed,
                               IBooleanInput pulse,
                               int pulseDebounceMillisecondsAtFullSpeed,
                               IBooleanOutput pulseMonitor,
                               float pulsesPerSecond,
                               IBooleanOutput secondsLamp)
        {
            // Check parameters:
            if (motor == null) { throw new ArgumentNullException(nameof(motor)); }
            if (minimumMotorSpeed <= 0f) { throw new ArgumentOutOfRangeException(nameof(minimumMotorSpeed)); }
            if (pulse == null) { throw new ArgumentNullException(nameof(pulse)); }
            if (pulseDebounceMillisecondsAtFullSpeed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pulseDebounceMillisecondsAtFullSpeed));
            }
            if (pulseMonitor == null) { throw new ArgumentNullException(nameof(pulseMonitor)); }
            if (pulsesPerSecond <= 0f) { throw new ArgumentOutOfRangeException(nameof(pulsesPerSecond)); }
            if (secondsLamp == null) { throw new ArgumentNullException(nameof(secondsLamp)); }

            // The minimal speed at which we are sure the motor is still turning at all (and not halting):
            const float MinimalSpeed = 0.07f;

            // The maximal speed the motor can handle:
            const float MaximalSpeed = 1.0f;

            // A rough guess for a good speed to reach the timing goals:
            const float InitialSpeedGuess = 0.2f;

            // Start a blinking seconds lamp calmly going on in one second and off again in the next:
            var secondsTimer = new Timer((state) => { secondsLamp.Value = !secondsLamp.Value; },
                                         null, 0, 1000);

            BooleanDebouncedInput debouncedPulse = pulse.Debounced(pulseDebounceMillisecondsAtFullSpeed);

            // Let the motor run until the pulse changes from false to true to initialize the position to a pulse
            // boundary:
            motor.Value = InitialSpeedGuess;
            debouncedPulse.DebounceMilliseconds = pulseDebounceMillisecondsAtFullSpeed;
            debouncedPulse.WaitFor(true, true);

            // This is our starting point:
            var clockStartTime = DateTime.UtcNow;
            var idealSecondsForCycle = 1f / pulsesPerSecond;
            int safetyDebounceMilliseconds = 10;

            // State variables, divided into "ideal" (t) and "actual" (a) values:

            // ------------------------------------------> time
            //      :    |        : |          |
            //      :    t0       : t1         t2
            //      :             :            :
            //      : ----v0----> : ----v1---> :
            //      a0            a1

            // v0 is the speed which should have brought the clock from a0 to t1 but brought it to a1.
            // v1 is the speed to be calculated needed to bring the clock from a1 to t2.

            // v1 = v0 * ((t1 - a0) / (t2 - a1)) * ((a1 - a0) / (t1 - a0))
            //    = v0 * (a1 - a0) / (t2 - a1)

            uint n;      // The number of cycles passed
            DateTime t0; // Ideal start of the running cycle
            DateTime t1; // Ideal end of the running cycle
            DateTime t2; // Ideal end of the next cycle
            DateTime a0; // Actual start of the running cycle
            DateTime a1; // Actual end of the running cycle

            n = 0;
            t0 = clockStartTime;
            a0 = t0;

            while (true)
            {
                // Run one cycle and measure time:
                n++;
                t1 = clockStartTime.AddSeconds(n * idealSecondsForCycle);
                Thread.Sleep(safetyDebounceMilliseconds);
                debouncedPulse.WaitFor(true, true);
                a1 = DateTime.UtcNow;

                // Calculate and apply the new speed:
                t2 = clockStartTime.AddSeconds((n + 1) * idealSecondsForCycle);
                double v1 = motor.Value * (a1 - a0).TotalSeconds / (t2 - a1).TotalSeconds;

                Console.WriteLine("n = " + n.ToString("N0") +
                                  "   v0 = " + motor.Value.ToString("N4") +
                                  "   v1 = " + v1.ToString("N4"));

                motor.Value = Math.Min(MaximalSpeed, Math.Max(MinimalSpeed, (float)v1));

                // The current cycle gets the passed one:
                t0 = t1;
                a0 = a1;
            }
        }
    }
}
