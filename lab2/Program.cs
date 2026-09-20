using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("--- Лабораторна робота №2 | Варіант 2 ---\n");

        Console.WriteLine(" Creating objects ");
        
        // 1. Створення об'єкта через конструктор за замовчуванням
        Car car1 = new Car();
        
        // 2. Створення об'єкта через параметризований конструктор
        Car car2 = new Car("Tesla", "Model S", 2022);

        // Виклик методів
        car1.StartEngine();
        car2.StartEngine();

        Console.WriteLine(" Objects created \n");

        // 3. Звільняємо посилання на об'єкти для збирача сміття
        car1 = null;
        car2 = null;

        Console.WriteLine(" End of Main, preparing for GC ");
        
        // Примусовий запуск збирача сміття (як у завданні)
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}

class Car
{
    // Приватні поля (згідно з завданням)
    private string _brand;
    private string _model;
    private int _year;

    // Властивості з валідацією (рік не може бути в майбутньому, наприклад більше 2026)
    public string Brand
    {
        get { return _brand; }
        set { _brand = value; }
    }

    public string Model
    {
        get { return _model; }
        set { _model = value; }
    }

    public int Year
    {
        get { return _year; }
        set 
        { 
            if (value > 2026) // Валідація: рік не може бути в майбутньому
            {
                Console.WriteLine("Помилка! Рік випуску не може бути в майбутньому. Встановлено 2020 рік за замовчуванням.");
                _year = 2020;
            }
            else
            {
                _year = value; 
            }
        }
    }

    // 1. Конструктор за замовчуванням, який викликає параметризований через : this(...)
    public Car() : this("Unknown", "Unknown", 2000)
    {
        Console.WriteLine("Викликано конструктор за замовчуванням.");
    }

    // 2. Параметризований конструктор
    public Car(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Console.WriteLine($"Викликано параметризований конструктор для {Brand} {Model}.");
    }

    // Метод, що виконує дію
    public void StartEngine()
    {
        Console.WriteLine($"Двигун автомобіля {Brand} {Model} ({Year} року) запущено! Врум-врум!");
    }

    // Деструктор (фіналізатор)
    ~Car()
    {
        Console.WriteLine($"Деструктор: Об'єкт автомобіля {Brand} {Model} видалено з пам'яті.");
    }
}
