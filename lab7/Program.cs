using System;

namespace Lab7V2
{
    public class Shape
    {
        public virtual double Area()
        {
            Console.WriteLine("Викликано метод Area() з класу Shape");
            return 0.0;
        }
    }

    public class Circle : Shape
    {
        public override double Area()
        {
            Console.WriteLine("Викликано метод Area() з класу Circle (override)");
            return 3.14;
        }
    }

    public class Triangle : Shape
    {
        public new double Area()
        {
            Console.WriteLine("Викликано метод Area() з класу Triangle (new)");
            return 0.5;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Shape obj1 = new Circle();
            Shape obj2 = new Triangle();

            Console.WriteLine("--- Using base class reference ---");
            obj1.Area(); 
            obj2.Area(); 

            Console.WriteLine("\n--- Using derived class reference ---");
            ((Circle)obj1).Area();
            ((Triangle)obj2).Area();
        }
    }
}
