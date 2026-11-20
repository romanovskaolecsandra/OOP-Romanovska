using System;
using System.Collections.Generic;

namespace IndependentWork12
{
    public static class DataGenerator
    {
        public static (List<Product> Products, List<Customer> Customers, List<Sale> Sales) GenerateData()
        {
            // Фіксований Seed для відтворюваності результатів
            var random = new Random(42);

            string[] categories = { "Електроніка", "Побутова техніка", "Аксесуари", "Офіс", "Смарт-дім" };
            string[] cities = { "Київ", "Львів", "Одеса", "Дніпро", "Харків", "Вінниця" };

            // 1. Генеруємо 60 продуктів
            var products = new List<Product>();
            for (int i = 1; i <= 60; i++)
            {
                string category = categories[random.Next(categories.Length)];
                products.Add(new Product
                {
                    Id = i,
                    Name = $"{category} Модель-{i:D2}",
                    Category = category,
                    UnitPrice = random.Next(3, 150) * 100m // від 300 до 14 900 грн
                });
            }

            // 2. Генеруємо 70 клієнтів
            var customers = new List<Customer>();
            for (int i = 1; i <= 70; i++)
            {
                customers.Add(new Customer
                {
                    Id = i,
                    Name = $"Клієнт #{i:D2}",
                    City = cities[random.Next(cities.Length)]
                });
            }

            // 3. Генеруємо 100 продажів за 2025-2026 роки
            var sales = new List<Sale>();
            var startDate = new DateTime(2025, 1, 1);
            for (int i = 1; i <= 100; i++)
            {
                sales.Add(new Sale
                {
                    Id = i,
                    ProductId = random.Next(1, products.Count + 1),
                    CustomerId = random.Next(1, customers.Count + 1),
                    Quantity = random.Next(1, 10),
                    SaleDate = startDate.AddDays(random.Next(0, 650))
                });
            }

            return (products, customers, sales);
        }
    }
}
