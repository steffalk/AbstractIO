using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample13SimplifiedDevelopment
    {
        public static void Run(IBooleanInput button, IBooleanOutput motor)
        {
            // Multithreading is simple:

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    motor.Value = button.Value;
                    System.Threading.Thread.Sleep(50);
                }
            });

            thread.Start();
        }
    }
}
