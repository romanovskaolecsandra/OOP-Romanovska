using System;

namespace IndependentWork1
{
    public class Employee
    {
        private string _name;
        private double _hourlyRate;

        public string Name
        {
            get { return _name; }
        }

        public Employee(string name, double hourlyRate)
        {
            _name = name;
            _hourlyRate = hourlyRate;
        }

        public double CalculateSalary(int hoursWorked)
        {
            return _hourlyRate * hoursWorked;
        }
    }

    public class Rectangle
    {
        private double _width;
        private double _height;

        public double Width
        {
            get { return _width; }
            set 
            { 
                if (value > 0) _width = value; 
            }
        }

        public Rectangle(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public double CalculateArea()
        {
            return _width * _height;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; 

            Console.WriteLine("=== Демонстрація роботи класів ===\n");

            Employee emp = new Employee("Олександр", 250.0); 
            int hours = 160;
            double salary = emp.CalculateSalary(hours);

            Console.WriteLine($"Працівник: {emp.Name}");
            Console.WriteLine($"Заробітна плата за {hours} год.: {salary:F2} грн.\n");

            Rectangle rect = new Rectangle(5.5, 10.0);
            double areaBefore = rect.CalculateArea();
            Console.WriteLine($"Прямокутник із початковою шириною ({rect.Width}):");
            Console.WriteLine($"Початкова площа прямокутника: {areaBefore:F2}\n");

            rect.Width = 7.0;
            double areaAfter = rect.CalculateArea();
            Console.WriteLine($"Змінено ширину прямокутника на: {rect.Width}");
            Console.WriteLine($"Оновлена площа прямокутника: {areaAfter:F2}");

            Console.ReadLine();
        }
    }
}
