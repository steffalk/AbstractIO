using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample12ClockWithContinuouslyControlledMotor
    {

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

            Calibrate(motor, pulse, millisecondsPerPulse);

            // Run forever:
            Thread.Sleep(Timeout.Infinite);
        }

        private static void Calibrate(ISingleOutput motor, IBooleanInput pulse, float millisecondsPerPulse)
        {
            // Let the motor run at full speed until the pulse changes from false to true:
            motor.Value = 1f;
            pulse.WaitFor(true, true);

            // Measure the time until pulse turns to true again:
            var start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var fastTime = DateTime.UtcNow - start;

            // Wait until shortly before the next pulse:
            Thread.Sleep((int)(fastTime.TotalMilliseconds / 10) * 9);

            // Let the motor run at 20% fullSpeedTime:
            motor.Value = 0.2f;
            pulse.WaitFor(true, true);

            start = DateTime.UtcNow;
            pulse.WaitFor(true, true);
            var slowTime = DateTime.UtcNow - start;
        }
    }
}
