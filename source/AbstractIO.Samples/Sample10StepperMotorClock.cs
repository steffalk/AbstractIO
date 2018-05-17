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

            // Variant 1: Sleep a minute, turn the stepper, sleep again:
            // RunUsingSleep(stepper, stepsPerMinute, pauseBetweenStepsInMs);

            // Variant 2: Use a timer fireing each minute:
            RunUsingTimer(stepper, stepsPerMinute, pauseBetweenStepsInMs);
        }

        private static void RunUsingSleep(
            IStepDrive stepper, int stepsPerMinute, int pauseBetweenStepsInMs)
        {
            // Save "now":
            DateTime lastTime = DateTime.UtcNow;

            while (true)
            {
                // Compute the next time at which the stepper must work:
                // For demo purposes, you can use one step per second, for example:
                // DateTime nextTime = lastTime.AddSeconds(1.0);
                DateTime nextTime = lastTime.AddMinutes(1.0);

                // Wait the correct time, avoiding cumulative errors:
                Thread.Sleep((int)((nextTime - DateTime.UtcNow).Ticks / TimeSpan.TicksPerMillisecond));

                // Let the stepper do its job to turn the clock:
                stepper.MoveSteps(stepsPerMinute, pauseBetweenStepsInMs);

                // Set this time as the base for the next period:
                lastTime = nextTime;

                // Let the stepper turn freely, save power, and make not even a silent noise caused by PWM:
                stepper.ReleaseHoldingTorque();
            }
        }

        private static void RunUsingTimer(
            IStepDrive stepper, int stepsPerMinute, int pauseBetweenStepsInMs)
        {
            // Do steps each minute, that is, every 60000 milliseconds:
            var timer = new Timer(
                (state) =>
                {
                    stepper.MoveSteps(stepsPerMinute, pauseBetweenStepsInMs);
                    stepper.ReleaseHoldingTorque();
                },
                null,
                60000,
                60000);

            // Sleep and let the timer do its job:
            for (; ; ) Thread.Sleep(1000);

        }

    }
}
