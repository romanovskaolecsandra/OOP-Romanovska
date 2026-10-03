using System;
using System.Collections.Generic;

namespace Lab8V2
{
    public class Vehicle
    {
        private double speed;

        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Vehicle(double speed)
        {
            this.speed = speed;
        }

        public virtual void Move()
        {
            Console.WriteLine("Транспортний засіб рухається зі швидкістю " + speed + " км/год");
        }
    }

    public class Car : Vehicle
    {
        private int numWheels;

        public int NumWheels
        {
            get { return numWheels; }
            set { numWheels = value; }
        }

        public Car(double speed, int numWheels) : base(speed)
        {
            this.numWheels = numWheels;
        }

        public override void Move()
        {
            Console.WriteLine("Автомобіль на " + numWheels + " колесах їде зі швидкістю " + Speed + " км/год");
        }
    }

    public class Bicycle : Vehicle
    {
        private bool hasGears;

        public bool HasGears
        {
            get { return hasGears; }
            set { hasGears = value; }
        }

        public Bicycle(double speed, bool hasGears) : base(speed)
        {
            this.hasGears = hasGears;
        }

        public override void Move()
        {
            string gearsInfo = hasGears ? "із передачами" : "без передач";
            Console.WriteLine("Велосипед " + gearsInfo + " їде зі швидкістю " + Speed + " км/год");
        }
    }

    public class Boat : Vehicle
    {
        private string engineType;

        public string EngineType
        {
            get { return engineType; }
            set { engineType = value; }
        }

        public Boat(double speed, string engineType) : base(speed)
        {
            this.engineType = engineType;
        }

        public override void Move()
        {
            Console.WriteLine("Човен із двигуном типу [" + engineType + "] пливе зі швидкістю " + Speed + " км/год");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Vehicle> vehicles = new List<Vehicle>();

            vehicles.Add(new Car(120.0, 4));
            vehicles.Add(new Bicycle(25.0, true));
            vehicles.Add(new Boat(45.0, "Стаціонарний"));

            Console.WriteLine("--- Перевірка поліморфного виклику методів ---");
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Move();
            }

            Console.WriteLine("\n--- Агрегація результатів ---");
            double totalSpeed = 0;
            for (int i = 0; i < vehicles.Count; i++)
            {
                totalSpeed += vehicles[i].Speed;
            }

            double averageSpeed = totalSpeed / vehicles.Count;
            Console.WriteLine("Середня швидкість усіх транспортних засобів: " + averageSpeed + " км/год");
        }
    }
}
