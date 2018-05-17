using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample09SimpleStepperMotor
    {
        /// <summary>
        /// Let a stepper turn randomly.
        /// </summary>
        /// <param name="stepper"></param>
        public static void Run(IStepDrive stepper)
        {
            if (stepper == null) throw new ArgumentNullException(nameof(stepper));

            var random = new Random();

            const int steps = 200;

            while (true)
            {
                stepper.MoveSteps(random.Next(steps * 2 + 1) - steps, 500);
                Thread.Sleep(1000);
            }
        }
    }
}
