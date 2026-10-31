using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab15
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" },
                new Category { Id = 3, Name = "Clothing" },
                new Category { Id = 4, Name = "Home & Kitchen" },
                new Category { Id = 5, Name = "Sports" }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Smartphone X", Price = 799.99m, CategoryId = 1 },
                new Product { Id = 2, Name = "Laptop Pro", Price = 1299.50m, CategoryId = 1 },
                new Product { Id = 3, Name = "Wireless Headphones", Price = 150.00m, CategoryId = 1 },
                new Product { Id = 4, Name = "4K TV", Price = 550.00m, CategoryId = 1 },
                new Product { Id = 5, Name = "C# Programming Guide", Price = 45.00m, CategoryId = 2 },
                new Product { Id = 6, Name = "History Encyclopedia", Price = 65.00m, CategoryId = 2 },
                new Product { Id = 7, Name = "Winter Jacket", Price = 120.00m, CategoryId = 3 },
                new Product { Id = 8, Name = "Running Shoes", Price = 85.50m, CategoryId = 5 },
                new Product { Id = 9, Name = "Coffee Maker", Price = 95.00m, CategoryId = 4 },
                new Product { Id = 10, Name = "Blender Ultra", Price = 110.00m, CategoryId = 4 },
                new Product { Id = 11, Name = "Smart Watch", Price = 250.00m, CategoryId = 1 },
                new Product { Id = 12, Name = "Fantasy Novel", Price = 20.00m, CategoryId = 2 },
                new Product { Id = 13, Name = "Leather Wallet", Price = 35.00m, CategoryId = 3 },
                new Product { Id = 14, Name = "Yoga Mat", Price = 25.00m, CategoryId = 5 },
                new Product { Id = 15, Name = "Gaming Mouse", Price = 75.00m, CategoryId = 1 }
            };

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("=== ВИКОНАННЯ LINQ-ЗАПИТІВ (ВАРІАНТ 1: ІНТЕРНЕТ-МАГАЗИН) ===\n");

            Console.WriteLine("--- Запит 1: Продукти в 'Electronics' з ціною > 500 ---");
            sw.Restart();
            var q1Query = from p in products
                          join c in categories on p.CategoryId equals c.Id
                          where c.Name == "Electronics" && p.Price > 500
                          select p;
            var res1Query = q1Query.ToList();
            sw.Stop();
            long t1Q = sw.ElapsedTicks;

            sw.Restart();
            var res1Method = products
                .Where(p => p.Price > 500 && categories.Any(c => c.Id == p.CategoryId && c.Name == "Electronics"))
                .ToList();
            sw.Stop();
            long t1M = sw.ElapsedTicks;

            foreach (var p in res1Query)
                Console.WriteLine($"- {p.Name} (${p.Price})");
            Console.WriteLine($"Час (Query): {t1Q} ticks | Час (Method): {t1M} ticks\n");

            Console.WriteLine("--- Запит 2: Об'єднання продуктів з категоріями ---");
            sw.Restart();
            var q2Query = from p in products
                          join c in categories on p.CategoryId equals c.Id
                          select new { ProdName = p.Name, CatName = c.Name };
            var res2Query = q2Query.ToList();
            sw.Stop();
            long t2Q = sw.ElapsedTicks;

            sw.Restart();
            var res2Method = products.Join(
                categories,
                p => p.CategoryId,
                c => c.Id,
                (p, c) => new { ProdName = p.Name, CatName = c.Name }
            ).ToList();
            sw.Stop();
            long t2M = sw.ElapsedTicks;

            foreach (var item in res2Query.Take(5))
                Console.WriteLine($"- Продукт: {item.ProdName} | Категорія: {item.CatName}");
            Console.WriteLine("... [виведено перші 5 елементів]");
            Console.WriteLine($"Час (Query): {t2Q} ticks | Час (Method): {t2M} ticks\n");

            Console.WriteLine("--- Запит 3: Середня ціна в кожній категорії ---");
            sw.Restart();
            var q3Query = from p in products
                          join c in categories on p.CategoryId equals c.Id
                          group p by c.Name into g
                          select new { Category = g.Key, AvgPrice = g.Average(p => p.Price) };
            var res3Query = q3Query.ToList();
            sw.Stop();
            long t3Q = sw.ElapsedTicks;

            sw.Restart();
            var res3Method = products.Join(categories, p => p.CategoryId, c => c.Id, (p, c) => new { p, c })
                .GroupBy(x => x.c.Name)
                .Select(g => new { Category = g.Key, AvgPrice = g.Average(x => x.p.Price) })
                .ToList();
            sw.Stop();
            long t3M = sw.ElapsedTicks;

            foreach (var item in res3Query)
                Console.WriteLine($"- Категорія: {item.Category} | Сер. ціна: ${item.AvgPrice:F2}");
            Console.WriteLine($"Час (Query): {t3Q} ticks | Час (Method): {t3M} ticks\n");

            Console.WriteLine("--- Запит 4: Найдорожчий продукт ---");
            sw.Restart();
            var maxPriceQuery = (from p in products select p.Price).Max();
            var res4Query = (from p in products where p.Price == maxPriceQuery select p).FirstOrDefault();
            sw.Stop();
            long t4Q = sw.ElapsedTicks;

            sw.Restart();
            var res4Method = products.OrderByDescending(p => p.Price).FirstOrDefault();
            sw.Stop();
            long t4M = sw.ElapsedTicks;

            if (res4Method != null)
                Console.WriteLine($"- Найдорожчий: {res4Method.Name} (${res4Method.Price})");
            Console.WriteLine($"Час (Query): {t4Q} ticks | Час (Method): {t4M} ticks\n");

            Console.WriteLine("========================================================\n");
        }
    }
}
