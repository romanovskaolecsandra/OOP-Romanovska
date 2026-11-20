using System;

namespace IndependentWork13
{
    public static class MathOperations
    {
        // Операція 1: Помірне навантаження (тригонометрія та логарифми)
        public static double HeavyMathTransform(int number)
        {
            double x = number + 1.0;
            double result = 0;
            for (int i = 1; i <= 15; i++)
            {
                result += Math.Sqrt(x * i) * Math.Sin(x / i) + Math.Log(x + i);
            }
            return result;
        }

        // Операція 2: Високе CPU-навантаження (перевірка, чи є число простим)
        public static bool IsPrime(int number)
        {
            if (number < 2) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            int boundary = (int)Math.Floor(Math.Sqrt(number));
            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
    }
}
