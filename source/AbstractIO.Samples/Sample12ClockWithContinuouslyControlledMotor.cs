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

            const float MinimalSpeed = 0.1f;

            // Start a blinking seconds lamp calmly going on in one second and off again in the next:
            var secondsTimer = new Timer((state) => { secondsLamp.Value = !secondsLamp.Value; },
                                         null, 0, 1000);

            BooleanDebouncedInput debouncedPulse = pulse.Debounced(pulseDebounceMillisecondsAtFullSpeed);

            // Let the motor run at full speed until the pulse changes from false to true:
            motor.Value = 1f;
            debouncedPulse.DebounceMilliseconds = pulseDebounceMillisecondsAtFullSpeed;
            debouncedPulse.WaitFor(true, true);

            // This is our staring point:
            var startTime = DateTime.UtcNow;
            var beginOfCurrentCycle = startTime;
            uint numberOfCycles = 0;
            var idealSecondsForCycle = 1f / pulsesPerSecond;

            // Initialize the speed:
            motor.Value = 0.2f;

            while (true)
            {
                // Calculate when the next cyle _should_ be finished:
                // Do not add some seconds every pulse but multiply and add to start time to avoid cumulative errors.
                // This is the time we want the next gear cycle to end, ideally:
                numberOfCycles++;
                var targetEndOfCurrentCycle = startTime.AddSeconds(numberOfCycles / idealSecondsForCycle);

                // Run the motor at the current speed until the next cycle and measure the time::
                debouncedPulse.WaitFor(true, true);
                var actualEndOfCurrentCycle = DateTime.UtcNow;

                // Calculate the new motor speed, so that the next cycle should end at the correct time:
                var secondsOfPassedCycle = (float)((actualEndOfCurrentCycle - beginOfCurrentCycle).TotalSeconds);
                var targetEndOfNextCycle = startTime.AddSeconds((numberOfCycles + 1) / idealSecondsForCycle);
                var secondsToEndOfNextCycle = (float)((targetEndOfNextCycle - actualEndOfCurrentCycle).TotalSeconds);
                // With motor.Value, a cycle took secondsOfPassedCycle seconds.
                // With which motor.Value will we realize secondsToEndOfNextCycle seconds?
                motor.Value = Math.Max(MinimalSpeed, motor.Value * secondsToEndOfNextCycle / secondsOfPassedCycle);

                // Turnover to the next cycle:
                beginOfCurrentCycle = actualEndOfCurrentCycle;
            }
        }
    }
}
