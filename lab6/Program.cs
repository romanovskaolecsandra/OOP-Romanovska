using System;

namespace Lab6V2
{
    public class Shape
    {
        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public Shape(string color)
        {
            this.color = color;
        }

        public virtual double GetArea()
        {
            return 0.0;
        }

        public string GetShapeType()
        {
            return "Generic Shape";
        }
    }

    public class Circle : Shape
    {
        private double radius;

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Circle(string color, double radius) : base(color)
        {
            this.radius = radius;
        }

        public override double GetArea()
        {
            return Math.PI * radius * radius;
        }

        public void Draw()
        {
            Console.WriteLine("[Draw] Малюємо КОЛО з радіусом " + radius + " та кольором " + Color + ".");
        }
    }

    public class Rectangle : Shape
    {
        private double width;
        private double height;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public Rectangle(string color, double width, double height) : base(color)
        {
            this.width = width;
            this.height = height;
        }

        public override double GetArea()
        {
            return width * height;
        }

        public void Draw()
        {
            Console.WriteLine("[Draw] Малюємо ПРЯМОКУТНИК розміром " + width + "x" + height + " та кольором " + Color + ".");
        }

        public new string GetShapeType()
        {
            return "Rectangle Shape";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Створення об'єктів ===");
            Shape genericShape = new Shape("Прозорий");
            Circle circle = new Circle("Червоний", 5.0);
            Rectangle rectangle = new Rectangle("Синій", 4.0, 6.0);

            Console.WriteLine("Базова фігура: колір = " + genericShape.Color);
            Console.WriteLine("Коло: колір = " + circle.Color + ", радіус = " + circle.Radius);
            Console.WriteLine("Прямокутник: колір = " + rectangle.Color + ", розмір = " + rectangle.Width + "x" + rectangle.Height);
            Console.WriteLine();

            Console.WriteLine("=== 2. Виклик унікальних методів ===");
            circle.Draw();
            rectangle.Draw();
            Console.WriteLine();

            Console.WriteLine("=== 3. Демонстрація поліморфізму (override) ===");
            Shape[] shapes = new Shape[2];
            shapes[0] = circle;
            shapes[1] = rectangle;

            for (int i = 0; i < shapes.Length; i++)
            {
                Console.WriteLine("Фігура типу " + shapes[i].GetType().Name + " має площу: " + shapes[i].GetArea());
            }
            Console.WriteLine();

            Console.WriteLine("=== 4. Різниця між override та new ===");
            
            Rectangle rectRef = rectangle;
            Shape shapeRef = rectangle;

            Console.WriteLine("--- Виклик OVERRIDE методу (GetArea) ---");
            Console.WriteLine("Через посилання Rectangle: " + rectRef.GetArea());
            Console.WriteLine("Через посилання Shape: " + shapeRef.GetArea());

            Console.WriteLine("\n--- Виклик NEW методу (GetShapeType) ---");
            Console.WriteLine("Через посилання Rectangle: " + rectRef.GetShapeType());
            Console.WriteLine("Через посилання Shape: " + shapeRef.GetShapeType());
        }
    }
}
