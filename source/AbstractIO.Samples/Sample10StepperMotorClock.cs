using System;
using System.Threading;
using AbstractIO;

namespace AbstractIO.Samples
{
    /// <summary>
    /// A simple stepper-driven clock: Let a stepper turn some steps each minute.
    /// For the mechanics, you can use any technical construction kit, such as fischertechnik.
    /// </summary>
    public static class Sample10StepperMotorClock
    {
        /// <summary>
        /// Runs a mechanical clock by turning a stepper motor periodically.
        /// </summary>
        /// <param name="stepper">The stepper to drive.</param>
        /// <param name="stepsPerMinute">The number of steps that the stepper shall perform each minute. This may be any
        /// positive or negative non-null number and depends on the mechanical implementation of the actual clock.</param>
        /// <param name="pauseBetweenStepsInMs">The number of milliseconds to wait between stepper steps. Set this
        /// parameter depending on the mechanical implementation.</param>
        public static void Run(IStepDrive stepper, int stepsPerMinute, int pauseBetweenStepsInMs)
        {
            if (stepper == null) throw new ArgumentNullException(nameof(stepper));
            if (stepsPerMinute == 0) throw new ArgumentOutOfRangeException(nameof(stepsPerMinute));

            // Save "now":
            DateTime lastTime = DateTime.UtcNow;

            while (true)
            {
                // Compute the next time at which the stepper must work:
                // For demo purposes, you can use one step per second, for example:
                // DateTime nextTime = lastTime.AddSeconds(1.0);
                DateTime nextTime = lastTime.AddMilliseconds(1.0);

                // Wait the correct time, avoiding cumulative errors:
                Thread.Sleep((nextTime - DateTime.UtcNow).Milliseconds);

                // Let the stepper do its job to turn the clock:
                stepper.MoveSteps(stepsPerMinute, pauseBetweenStepsInMs);

                // Set this time as the base for the next period:
                lastTime = nextTime;

                // Let the stepper turn freely, save power, and make not even a silent noise caused by PWM:
                stepper.ReleaseHoldingTorque();
            }
        }
    }
}
