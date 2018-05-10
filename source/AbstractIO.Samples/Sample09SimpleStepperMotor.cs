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

            while (true)
            {
                stepper.MoveSteps(random.Next(101) - 50, 0);
                Thread.Sleep(1000);
            }
        }
    }
}
