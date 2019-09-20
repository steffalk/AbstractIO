namespace AbstractIO.Samples
{
    public static class Sample13SimplifiedDevelopment
    {
        public static void Run(IBooleanInput button, IBooleanOutput motor)
        {
            while (true)
            {
                motor.Value = button.Value;
            }
        }
    }
}
