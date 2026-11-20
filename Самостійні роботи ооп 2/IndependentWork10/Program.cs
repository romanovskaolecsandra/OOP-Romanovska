using System;
using System.Collections.Generic;
using Lab9Self;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ================= 1. КОЛЕКЦІЯ ЧИСЕЛ (List<int>) =================
Console.WriteLine("=== 1. Робота з List<int> ===");
var numbers = new List<int> { 3, 12, 7, 25, 4, 18, 9 };

var squares = CollectionUtils.Map(numbers, n => n * n);
Console.WriteLine("Квадрати чисел (Map): " + string.Join(", ", squares));

var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
Console.WriteLine("Парні числа (Filter): " + string.Join(", ", evenNumbers));

int maxByLastDigit = CollectionUtils.MaxBy(numbers, n => n % 10);
Console.WriteLine($"Число з найбільшою останньою цифрою (MaxBy): {maxByLastDigit}");

Console.Write("Вивід чисел зі збільшенням на 10 (ForEach): ");
CollectionUtils.ForEach(evenNumbers, n => Console.Write($"{n + 10} "));
Console.WriteLine("\n");

// ================= 2. КОЛЕКЦІЯ РЯДКІВ (List<string>) =================
Console.WriteLine("=== 2. Робота з List<string> ===");
var words = new List<string> { "C#", "Delegates", "Lambda", "LINQ", "Collections" };

var lengths = CollectionUtils.Map(words, w => $"{w} ({w.Length})");
Console.WriteLine("Довжина слів (Map): " + string.Join(", ", lengths));

var longWords = CollectionUtils.Filter(words, w => w.Length > 5);
Console.WriteLine("Слова довші за 5 символів (Filter): " + string.Join(", ", longWords));

string longestWord = CollectionUtils.MaxBy(words, w => w.Length);
Console.WriteLine($"Найдовше слово (MaxBy): {longestWord}");

Console.Write("У верхньому регістрі (ForEach): ");
CollectionUtils.ForEach(words, w => Console.Write($"{w.ToUpper()} "));
Console.WriteLine("\n");

// ================= 3. КОЛЕКЦІЯ ОБ'ЄКТІВ (List<Product>) =================
Console.WriteLine("=== 3. Робота з List<Product> ===");
var products = new List<Product>
{
    new Product { Name = "Laptop", Price = 28000 },
    new Product { Name = "Mouse", Price = 650 },
    new Product { Name = "Keyboard", Price = 1800 },
    new Product { Name = "Monitor", Price = 9500 }
};

var productNames = CollectionUtils.Map(products, p => p.Name);
Console.WriteLine("Назви товарів (Map): " + string.Join(", ", productNames));

var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"Найдорожчий товар (MaxBy): {mostExpensive.Name} — {mostExpensive.Price} грн");

var expensiveProducts = CollectionUtils.Filter(products, p => p.Price > 1500);
Console.WriteLine("Товари дорожчі за 1500 грн (Filter + ForEach):");
CollectionUtils.ForEach(expensiveProducts, p => Console.WriteLine($"  - {p.Name}: {p.Price} грн"));

public class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
