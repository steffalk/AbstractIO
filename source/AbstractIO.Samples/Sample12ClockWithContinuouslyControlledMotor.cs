using System;
using System.Diagnostics;

namespace AbstractIO.Samples
{
    /// <summary>
    /// Calculation of running averages: You define a capacity n and then add values. The average of the last n added
    /// values can then be computed.
    /// </summary>
    internal class RunningAverageCalculator
    {
        /// <summary>
        /// A container for the values to keep.
        /// </summary>
        private double[] _values;

        /// <summary>
        /// The index in <see cref="_values"/> at which the next value is to be put.
        /// </summary>
        private int _nextWriteIndex;

        /// <summary>
        /// The number of valid entries in <see cref="_values"/>.
        /// </summary>
        private int _count;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="capacity">The number of values over which the average shall be computed. This must be greater
        /// than zero.</param>
        public RunningAverageCalculator(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            _values = new double[capacity];
        }

        /// <summary>
        /// Gets the capacity of the calculator, that is, the number of values added through the
        /// <see cref="Add(double)"/> method over which the average shall be computed.
        /// </summary>
        public int Capacity
        {
            get
            {
                return _values.Length;
            }
        }

        /// <summary>
        /// Gets the number of values currently used for average calculation. This is initially 0 and increments up to
        /// <see cref="Capacity"/> (but not farer) with each call of <see cref="Add(double)"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
        }

        /// <summary>
        /// Adds a value to the set of values over which the average shall be computed.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add(double value)
        {
            _values[_nextWriteIndex] = value;
            _nextWriteIndex = (_nextWriteIndex + 1) % Capacity;
            if (_count < Capacity)
            {
                _count++;
            }
        }

        /// <summary>
        /// Gets the average over the last <see cref="Capacity"/> values added using the <see cref="Add(double)"/>
        /// method.
        /// </summary>
        public double Average
        {
            get
            {
                double sum = 0.0;
                for (int i = 0; i < Count; i++)
                {
                    sum += _values[i];
                }
                return sum / Count;
            }
        }

        /// <summary>
        /// Tests this class and throws an exception if a test fails.
        /// </summary>
        public static void Test()
        {
            void AssertIntAreEqual(int expected, int actual)
            {
                if (actual != expected)
                {
                    throw new Exception(
                        "Assertion error: Expected: " + expected.ToString() + "; actual: " + actual.ToString());
                }
            }

            void AssertDoubleAreEqual(double expected, double actual)
            {
                if (actual != expected)
                {
                    throw new Exception(
                        "Assertion error: Expected: " + expected.ToString() + "; actual: " + actual.ToString());
                }
            }

            RunningAverageCalculator c;

            // Run tests with capacity 1:

            c = new RunningAverageCalculator(1);
            AssertIntAreEqual(1, c.Capacity);
            AssertIntAreEqual(0, c.Count);

            c.Add(1.0);
            AssertIntAreEqual(1, c.Count);
            AssertDoubleAreEqual(1.0, c.Average);

            c.Add(2.0);
            AssertIntAreEqual(1, c.Count);
            AssertDoubleAreEqual(2.0, c.Average);

            // Run tests with capacity 2:

            c = new RunningAverageCalculator(2);
            AssertIntAreEqual(2, c.Capacity);
            AssertIntAreEqual(0, c.Count);

            c.Add(1.0);
            AssertIntAreEqual(1, c.Count);
            AssertDoubleAreEqual(1.0, c.Average);

            c.Add(2.0);
            AssertIntAreEqual(2, c.Count);
            AssertDoubleAreEqual((1.0 + 2.0) / 2, c.Average);

            c.Add(3.0);
            AssertIntAreEqual(2, c.Count);
            AssertDoubleAreEqual((3.0 + 2.0) / 2, c.Average);

            c.Add(4.0);
            AssertIntAreEqual(2, c.Count);
            AssertDoubleAreEqual((3.0 + 4.0) / 2, c.Average);

            c.Add(5.0);
            AssertIntAreEqual(2, c.Count);
            AssertDoubleAreEqual((5.0 + 4.0) / 2, c.Average);

            // Run tests with capacity 3:

            c = new RunningAverageCalculator(3);
            AssertIntAreEqual(3, c.Capacity);
            AssertIntAreEqual(0, c.Count);

            c.Add(1.0);
            AssertIntAreEqual(1, c.Count);
            AssertDoubleAreEqual(1.0, c.Average);

            c.Add(2.0);
            AssertIntAreEqual(2, c.Count);
            AssertDoubleAreEqual((1.0 + 2.0) / 2, c.Average);

            c.Add(3.0);
            AssertIntAreEqual(3, c.Count);
            AssertDoubleAreEqual((1.0 + 2.0 + 3.0) / 3, c.Average);

            c.Add(4.0);
            AssertIntAreEqual(3, c.Count);
            AssertDoubleAreEqual((4.0 + 2.0 + 3.0) / 3, c.Average);

            c.Add(5.0);
            AssertIntAreEqual(3, c.Count);
            AssertDoubleAreEqual((4.0 + 5.0 + 3.0) / 3, c.Average);

            c.Add(6.0);
            AssertIntAreEqual(3, c.Count);
            AssertDoubleAreEqual((4.0 + 5.0 + 6.0) / 3, c.Average);

            c.Add(7.0);
            AssertIntAreEqual(3, c.Count);
            AssertDoubleAreEqual((7.0 + 5.0 + 6.0) / 3, c.Average);
        }
    }

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
                               double idealSecondsPerCycle,
                               IBooleanInput runAtFullSpeedSwitch)
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


            // Run unit tests on the RunningAverageCalculator class:
            Debug.WriteLine("Testing RunningAverageCalculator");
            RunningAverageCalculator.Test();
            Debug.WriteLine("RunningAverageCalculator successfully tested");

            // An average calculator the motor output voltage (ranging from 0.0f to 1.0f) needed to read one cycle in
            // idealSecondsPerCycle seconds:
            var voltageForIdealCycleTime = new RunningAverageCalculator(10);

            // Add the initial guess of that voltage:
            voltageForIdealCycleTime.Add(initialSpeedGuess);

            // Give a short full speed pulse to the motor to get it surely running:
            motor.Value = 1.0f;
            System.Threading.Thread.Sleep(10);

            // Let the motor run until the pulse changes from false to true to initialize the position to a pulse
            // boundary:
            Debug.WriteLine("Initializing to pulse position");
            motor.Value = initialSpeedGuess;
            pulse.WaitFor(true, true);

            // This is our starting point:
            var clockStartTime = DateTime.UtcNow;

            int n = 0;                    // The number of cycles passed
            DateTime t0 = clockStartTime; // Ideal start of the running cycle
            DateTime a0 = t0;             // Actual start of the running cycle

            Debug.WriteLine("Ideal seconds per cycle = " + idealSecondsPerCycle.ToString("N4"));
            Debug.WriteLine("Running the clock at initial v = " + initialSpeedGuess.ToString("N4"));

            while (true)
            {
                if (runAtFullSpeedSwitch.Value)
                {
                    Debug.WriteLine("Manually adjusting clock by running at full speed");

                    // Let the motor run at full speed to adjust the clock's time on the user's request:
                    float lastSpeed = motor.Value;
                    motor.Value = 1f;
                    runAtFullSpeedSwitch.WaitFor(false);

                    // Reinitialize:
                    Debug.WriteLine("Initializing to pulse position");
                    motor.Value = lastSpeed;
                    pulse.WaitFor(true, true);
                    Debug.WriteLine("Pulse reached");
                    n = 0;
                    clockStartTime = DateTime.UtcNow;
                    t0 = clockStartTime;
                    a0 = t0;
                }

                // Calculate the end of the current (and the beginning of the next) cylce:
                n++;
                DateTime t1 = clockStartTime.AddSeconds(idealSecondsPerCycle * n);
                double t1a0 = (t1 - a0).TotalSeconds;

                // Wait for the next (debounced) pulse, telling us that we reached the end of the current cycle:
                DateTime a1;     // The actual end of the current cycle.
                double a1a0;     // a1 - a0: The number of seconds between a0 and a1.
                int bounces = 0; // The number of bounces the pulse contacts made

                do
                {
                    pulse.WaitFor(true, true);
                    a1 = DateTime.UtcNow;
                    a1a0 = (a1 - a0).TotalSeconds;
                    bounces++;
                } // Debounce by accepting the next pulse not earlier than at 70% of the wanted time interval:
                while (a1a0 < 0.7 * t1a0);

                // We may have missed one or more pulses due to mechanical errors in pulse detection.
                // Estimate the number of real cyles, rounding by adding 0.5 and casting to int (which truncates):
                int cycles = (int)((a1 - t1).TotalSeconds * motor.Value /
                                   (voltageForIdealCycleTime.Average * idealSecondsPerCycle)
                                   + 0.5)
                             + 1;

                if (cycles > 1)
                {
                    // We lost [cycles - 1] pulses. The worm turned multiple times until we got a contact.
                    // Adjust the counted pulses and the ideal target time for that number of pulses since the last
                    // contact:
                    n = n + cycles - 1;
                    t1 = clockStartTime.AddSeconds(idealSecondsPerCycle * n);
                }

                // Take note of the current measurement's insight:
                voltageForIdealCycleTime.Add(motor.Value * a1a0 / (idealSecondsPerCycle * cycles));

                // Calculate the motor voltage needed to reach the next cycle pulse right in time t1 and
                // set the motor voltage to this value, taking the lower and upper bounds into account:

                DateTime t2 = clockStartTime.AddSeconds(idealSecondsPerCycle * (n + 1));

                motor.Value =
                    Math.Max(minimumMotorSpeed,
                             Math.Min(1.0f,
                                      (float)(voltageForIdealCycleTime.Average * idealSecondsPerCycle
                                              / (t2 - a1).TotalSeconds)));

                // Report to debugger:
                double diff = (a1 - t1).TotalSeconds;
                // Math.Abs(double) is not implemented on Netduiono 3:
                double absDiff = diff < 0.0 ? -diff : diff;

                Debug.WriteLine(
                    "n = " + n.ToString("N0").PadLeft(8) +
                    " | bounces = " + bounces.ToString().PadLeft(3) +
                    " | cycles = " + cycles.ToString().PadLeft(2) +
                    " | vi = " + voltageForIdealCycleTime.Average.ToString("N4").PadLeft(6) +
                    " | t1 = " + t1.ToString("HH:mm:ss") +
                    " | a1 = " + a1.ToString("HH:mm:ss") +
                    " | " + (diff == 0.0 ? "exactly in time           " :
                             ((diff < 0.0 ? "early by " : " late by ") + absDiff.ToString("N4").PadLeft(7) + "s (" +
                              (absDiff * 100.0 / t1a0).ToString("N2").PadLeft(5) + "%)")) +
                    " | v = " + motor.Value.ToString("N4"));

                // The current cycle gets the passed one:
                t0 = t1;
                a0 = a1;
            }
        }
    }
}
