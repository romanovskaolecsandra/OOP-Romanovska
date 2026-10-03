using System;

namespace IndependentWork2
{
    public class Product
    {
        private int _id;
        private string _name;
        private decimal _price;
        private string _category;
        private int _stockCount;

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public decimal Price { get { return _price; } }
        public string Category { get { return _category; } }
        public int StockCount { get { return _stockCount; } }

        public Product(int id, string name, decimal price, string category, int stockCount)
        {
            _id = id;
            _name = name;
            _price = price;
            _category = category;
            _stockCount = stockCount;
        }

        public Product(int id, string name, decimal price) 
            : this(id, name, price, "Без категорії", 0)
        {
        }

        public Product(Product other) 
            : this(other._id, other._name, other._price, other._category, other._stockCount)
        {
        }

        public override string ToString()
        {
            return $"ID: {Id}, Назва: {Name}, Ціна: {Price:C}, Категорія: {Category}, Кількість: {StockCount}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Створення товарів ===\n");

            Product p1 = new Product(101, "Ноутбук", 35000.00m, "Електроніка", 15);
            Console.WriteLine("Товар 1 (Основний конструктор):");
            Console.WriteLine(p1.ToString() + "\n");

            Product p2 = new Product(102, "Мишка", 800.00m);
            Console.WriteLine("Товар 2 (Скорочений конструктор):");
            Console.WriteLine(p2.ToString() + "\n");

            Product p3 = new Product(p1);
            Console.WriteLine("Товар 3 (Конструктор копіювання з Товару 1):");
            Console.WriteLine(p3.ToString());

            Console.ReadLine();
        }
    }
}
