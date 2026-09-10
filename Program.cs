using System;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== Лабораторна робота №1 | Варіант 2 ===\n");

// Створення 3-х об'єктів
Car car1 = new Car("Tesla", "Model S", 2022);
Car car2 = new Car("BMW", "M5", 2021);
Car car3 = new Car("Toyota", "Camry", 2023);

// Виклик методів
car1.Drive(); car2.Drive(); car3.Drive();

// Опис класу Car згідно з варіантом
class Car
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public Car(string brand, string model, int year)
    {
        Brand = brand; Model = model; Year = year;
    }

    ~Car() { } // Деструктор

    public void Drive() => Console.WriteLine($"Автомобіль {Brand} {Model} ({Year}) вирушив у поїздку.");
}
