namespace AbstractIO
{
    public static class Math
    {
        public static int Min(int a, int b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static int Max(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static int Abs(int value)
        {
            if (value < 0)
            {
                return -value;
            }
            else
            {
                return value;
            }
        }

        public static float Min(float a, float b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static float Max(float a, float b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        public static float Abs(float value)
        {
            if (value < 0)
            {
                return -value;
            }
            else
            {
                return value;
            }
        }

        public static double Min(double a, double b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static double Max(double a, double b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static double Abs(double value)
        {
            if (value < 0)
            {
                return -value;
            }
            else
            {
                return value;
            }
        }
    }
}
