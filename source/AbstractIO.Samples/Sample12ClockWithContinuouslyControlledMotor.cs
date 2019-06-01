using System;

namespace AbstractIO.Samples
{
    public static class Sample12ClockWithContinuouslyControlledMotor
    {
        /// <summary>
        /// Runs a clock driven by a simple DC motor.
        /// </summary>
        /// <param name="motor">The motor to drive continuously.</param>
        /// <param name="minimumMotorSpeed">The minimum speed setting that causes the motor to turn. Speeds below this
        /// threshold may cause the motor to not turn at all.</param>
        /// <param name="initialSpeedGuess">A rough initial guess for a speed to try to reach the first cycle in time.
        /// </param>
        /// <param name="pulse">The input which pulses to measure the motor speed.</param>
        /// <param name="pulseDebounceMillisecondsAtFullSpeed">The time, in milliseconds, that shall be used as the
        /// debounce time for the <paramref name="pulse"/> input when the <paramref name="motor"/> runs at full speed.
        /// </param>
        /// <param name="pulseMonitor">An output to show the monitored pulse input.</param>
        /// <param name="idealSecondsPerCycle">The number of seconds for one pulse cycle which would give a perfectly
        /// accurate operation of the clock.</param>
        /// <remarks>The motor speed is constantly adapted to the measurement given by the pulse to realize the needed
        /// pulse times without cumulative errors, even if the motor changes its behaviour during the operation.
        /// </remarks>
        public static void Run(ISingleOutput motor,
                               float minimumMotorSpeed,
                               float initialSpeedGuess,
                               IBooleanInput pulse,
                               double idealSecondsPerCycle)
        {
            // Check parameters:
            if (motor == null)
            {
                throw new ArgumentNullException(nameof(motor));
            }
            if (minimumMotorSpeed <= 0f || minimumMotorSpeed >= 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumMotorSpeed));
            }
            if (initialSpeedGuess < minimumMotorSpeed || initialSpeedGuess > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(initialSpeedGuess));
            }
            if (pulse == null)
            {
                throw new ArgumentNullException(nameof(pulse));
            }
            if (idealSecondsPerCycle <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(idealSecondsPerCycle));
            }

            // Let the motor run until the pulse changes from false to true to initialize the position to a pulse
            // boundary:
            Console.WriteLine("Initializing to pulse position");
            motor.Value = initialSpeedGuess;
            pulse.WaitFor(true, true);

            // This is our starting point:
            var clockStartTime = DateTime.UtcNow;

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

            int n;       // The number of cycles passed
            DateTime t0; // Ideal start of the running cycle
            DateTime t1; // Ideal end of the running cycle
            DateTime t2; // Ideal end of the next cycle
            DateTime a0; // Actual start of the running cycle
            DateTime a1; // Actual end of the running cycle

            n = 0;
            t0 = clockStartTime;
            a0 = t0;

            Console.WriteLine("Performing first cycle");

            while (true)
            {
                // Run one cycle and measure time:
                n++;
                t1 = clockStartTime.AddSeconds(n * idealSecondsPerCycle);

                double a1a0;
                int bounces = -1;

                do
                {
                    pulse.WaitFor(true, true);
                    a1 = DateTime.UtcNow;
                    a1a0 = (a1 - a0).TotalSeconds;

                    // Perform a kind of additional debouncing by not accepting the next pulse earlier than at half of
                    // the wanted time interval:
                    bounces++;
                } while (a1a0 / (t1 - a0).TotalSeconds < 0.5);

                // We may miss a pulse due to mechanical errors in pulse detection.
                // Estimate the number of missed pulses, rounding by adding 0.5 and casting to int (which truncates):
                int turns = Math.Max(1, (int)((a1 - t0).TotalSeconds / idealSecondsPerCycle + 0.5));

                if (turns > 1)
                {
                    // We lost [turns - 1] pulses. The worm turned multiple times until we got a contact.
                    // Adjust the counted pulses and the ideal target time for that number of pulses since the last
                    // contact:
                    n = n + turns - 1;
                    t1 = clockStartTime.AddSeconds(n * idealSecondsPerCycle);
                }

                // Calculate the time we want the following cycle to end at:
                t2 = clockStartTime.AddSeconds((n + 1) * idealSecondsPerCycle);

                // Calculate and apply the new speed needed to reach that goal:
                double v1 = motor.Value * a1a0 / ((t2 - a1).TotalSeconds * turns);

                double diff = (t1 - a1).TotalSeconds;
                // Math.Abs(double) is not implemented on Netduiono 3:
                if (diff < 0.0)
                {
                    diff = -diff;
                }

                Console.WriteLine("n = " + n.ToString("N0").PadLeft(8) +
                                  " | bounces = " + bounces.ToString() +
                                  " | turns = " + turns.ToString() +
                                  (a1 < t1 ? " | early" : " |  late") +
                                  " by " + diff.ToString("N4") +
                                  "s (" + (diff * 100.0 / (t1 - a0).TotalSeconds).ToString("N1").PadLeft(5) +
                                  "%) | v0 = " + motor.Value.ToString("N4") +
                                  " | v1 = " + v1.ToString("N4"));

                motor.Value = Math.Min(1f, Math.Max(minimumMotorSpeed, (float)v1));

                // The current cycle gets the passed one:
                t0 = t1;
                a0 = a1;
            }
        }
    }
}
