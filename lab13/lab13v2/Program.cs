using System;
using System.Collections.Generic;

namespace Lab13
{
    public static class FunctionalUtils
    {
        public static List<TResult> Map<T, TResult>(List<T> list, Func<T, TResult> mapper)
        {
            var result = new List<TResult>();
            foreach (var item in list)
            {
                result.Add(mapper(item));
            }
            return result;
        }

        public static List<T> Filter<T>(List<T> list, Func<T, bool> predicate)
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static T MaxBy<T, TKey>(List<T> list, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (list == null || list.Count == 0) return default!;

            T maxItem = list[0];
            TKey maxKey = keySelector(maxItem);

            foreach (var item in list)
            {
                TKey key = keySelector(item);
                if (key.CompareTo(maxKey) > 0)
                {
                    maxKey = key;
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public static T MinBy<T, TKey>(List<T> list, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (list == null || list.Count == 0) return default!;

            T minItem = list[0];
            TKey minKey = keySelector(minItem);

            foreach (var item in list)
            {
                TKey key = keySelector(item);
                if (key.CompareTo(minKey) < 0)
                {
                    minKey = key;
                    minItem = item;
                }
            }
            return minItem;
        }

        public static T? FirstOrDefault<T>(List<T> list, Func<T, bool> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return default;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString() => $"{Name} (${Price})";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- РОБОТА З ТИПОМ INT ---");
            List<int> numbers = new List<int> { 5, 12, 3, 25, 8 };
            List<int> evenNumbers = FunctionalUtils.Filter(numbers, n => n % 2 == 0);
            Console.WriteLine($"Парні числа: {string.Join(", ", evenNumbers)}");
            List<int> squaredNumbers = FunctionalUtils.Map(numbers, n => n * n);
            Console.WriteLine($"Квадрати чисел: {string.Join(", ", squaredNumbers)}");

            Console.WriteLine("\n--- РОБОТА З ТИПОМ STRING ---");
            List<string> fruits = new List<string> { "Яблуко", "Банан", "Апельсин", "Ківі" };
            string longestFruit = FunctionalUtils.MaxBy(fruits, f => f.Length);
            Console.WriteLine($"Найдовша назва: {longestFruit}");
            string? firstWithB = FunctionalUtils.FirstOrDefault(fruits, f => f.StartsWith("Б"));
            Console.WriteLine($"Перше слово на 'Б': {firstWithB}");

            Console.WriteLine("\n--- РОБОТА З КАСТОМНИМ КЛАСОМ PRODUCT ---");
            List<Product> products = new List<Product>
            {
                new Product("Laptop", 1200.50),
                new Product("Phone", 699.99),
                new Product("Headphones", 150.00),
                new Product("Mouse", 25.50)
            };
            List<string> productNames = FunctionalUtils.Map(products, p => p.Name);
            Console.WriteLine($"Список назв: {string.Join(", ", productNames)}");
            List<Product> cheapProducts = FunctionalUtils.Filter(products, p => p.Price < 200);
            Console.WriteLine("Бюджетні товари: " + string.Join(", ", cheapProducts));
            Product mostExpensive = FunctionalUtils.MaxBy(products, p => p.Price);
            Product cheapest = FunctionalUtils.MinBy(products, p => p.Price);
            Console.WriteLine($"Найдорожчий товар: {mostExpensive}");
            Console.WriteLine($"Найдешевший товар: {cheapest}");
        }
    }
}
