using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample05ControlLampBrightnessThroughAnalogInput
    {
        public static void Run(IDoubleInput input, IDoubleOutput lamp)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (lamp == null) throw new ArgumentNullException(nameof(lamp));

            while (true)
            {
                lamp.Value = input.Value;
                Thread.Sleep(100); // Only to give you a chance to redeploy usung firmware as of 2018-04-08.
            }
        }
    }
}
