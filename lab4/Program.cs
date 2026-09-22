using System;

namespace Lab4Fraction
{
    public class Fraction
    {
        private int _numerator;
        private int _denominator;

        public static Fraction One => new Fraction(1, 1);

        public int Numerator
        {
            get => _numerator;
            set => _numerator = value;
        }

        public int Denominator
        {
            get => _denominator;
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("Знаменник не може дорівнювати нулю!");
                }
                _denominator = value;
            }
        }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static Fraction operator +(Fraction f1, Fraction f2)
        {
            if (f1 == null || f2 == null)
                throw new ArgumentNullException("Дріб не може бути null");

            int newNumerator = f1.Numerator * f2.Denominator + f2.Numerator * f1.Denominator;
            int newDenominator = f1.Denominator * f2.Denominator;
            return new Fraction(newNumerator, newDenominator);
        }

        public static Fraction operator *(Fraction f1, Fraction f2)
        {
            if (f1 == null || f2 == null)
                throw new ArgumentNullException("Дріб не може бути null");

            return new Fraction(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator);
        }

        public static bool operator ==(Fraction f1, Fraction f2)
        {
            if (ReferenceEquals(f1, f2)) return true;
            if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null)) return false;

            return f1.Numerator * f2.Denominator == f2.Numerator * f1.Denominator;
        }

        public static bool operator !=(Fraction f1, Fraction f2)
        {
            return !(f1 == f2);
        }

        public override bool Equals(object obj)
        {
            Fraction other = obj as Fraction;
            if (other != null)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _numerator.GetHashCode();
                hash = hash * 23 + _denominator.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{_numerator}/{_denominator}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Fraction f1 = new Fraction(1, 2);
            Fraction f2 = new Fraction(2, 3);
            
            Console.WriteLine($"Перший дріб (f1): {f1}");
            Console.WriteLine($"Другий дріб (f2): {f2}");

            Fraction staticOne = Fraction.One;
            Console.WriteLine($"Статична властивість Fraction.One: {staticOne}");

            Fraction sum = f1 + f2;
            Fraction product = f1 * f2;

            Console.WriteLine($"\nРезультат додавання (f1 + f2): {sum}");
            Console.WriteLine($"Результат множення (f1 * f2): {product}");

            Fraction f3 = new Fraction(2, 4);
            Console.WriteLine($"\nНовий дріб (f3): {f3}");
            Console.WriteLine($"f1 == f3: {f1 == f3}");
            Console.WriteLine($"f1 != f2: {f1 != f2}");
            Console.WriteLine($"f1.Equals(f3): {f1.Equals(f3)}");

            Console.WriteLine("\nСпроба створити дріб із нульовим знаменником:");
            try
            {
                Fraction badFraction = new Fraction(5, 0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка перехоплена успішно: {ex.Message}");
            }
        }
    }
}