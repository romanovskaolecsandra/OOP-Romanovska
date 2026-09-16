using System;

public class Car
{
    private string _brand;
    private string _model;
    private int _year;

    public string Brand { get => _brand; set => _brand = string.IsNullOrWhiteSpace(value) ? "Unknown" : value; }
    public string Model { get => _model; set => _model = string.IsNullOrWhiteSpace(value) ? "Unknown" : value; }
    public int Year
    {
        get => _year;
        set => _year = (value > DateTime.Now.Year) ? 2000 : value;
    }

    public Car() : this("Unknown", "Unknown", 2000) { }

    public Car(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
    }

    public void Drive() => Console.WriteLine($"Car: {Brand} {Model} ({Year})");

    ~Car() => Console.WriteLine($"[GC] Destroyed: {Brand} {Model}");
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Car car1 = new Car("Tesla", "Model S", 2022);
        car1.Drive();

        Car car2 = new Car();
        car2.Drive();

        Car car3 = new Car("Toyota", "Camry", 2030);
        car3.Drive();

        car1 = null; car2 = null; car3 = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
