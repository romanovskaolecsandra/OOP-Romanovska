using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork12
{
    public static class AnalyticsPipelines
    {
        // Пайплайн 1: Найпопулярніші продукти за категоріями (Топ-3)
        public static void TopProductsByCategory(List<Sale> sales, List<Product> products)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(s => s.Quantity)
                })
                .Join(
                    products,
                    stat => stat.ProductId,
                    p => p.Id,
                    (stat, p) => new
                    {
                        p.Name,
                        p.Category,
                        stat.TotalQuantity
                    })
                .GroupBy(x => x.Category)
                .Select(catGroup => new
                {
                    Category = catGroup.Key,
                    Top3 = catGroup
                        .OrderByDescending(x => x.TotalQuantity)
                        .Take(3)
                        .ToList()
                })
                .OrderBy(x => x.Category)
                .ToList();

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 1: Найпопулярніші продукти за категоріями (Час: {sw.ElapsedTicks} ticks / {sw.Elapsed.TotalMilliseconds:F3} ms) ===");
            foreach (var group in result)
            {
                Console.WriteLine($"Категорія: {group.Category}");
                foreach (var item in group.Top3)
                {
                    Console.WriteLine($"  - {item.Name}: {item.TotalQuantity} шт.");
                }
            }
            Console.WriteLine();
        }

        // Пайплайн 2: Загальний дохід за містами клієнтів
        public static void TotalRevenueByCity(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
                .Join(
                    customers,
                    sp => sp.CustomerId,
                    c => c.Id,
                    (sp, c) => new { c.City, sp.Revenue })
                .GroupBy(x => x.City)
                .Select(g => new
                {
                    City = g.Key,
                    TotalRevenue = g.Sum(x => x.Revenue),
                    OrdersCount = g.Count()
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 2: Загальний дохід за містами клієнтів (Час: {sw.ElapsedTicks} ticks / {sw.Elapsed.TotalMilliseconds:F3} ms) ===");
            foreach (var item in result)
            {
                Console.WriteLine($"  {item.City,-10} | Дохід: {item.TotalRevenue,12:N2} грн | Замовлень: {item.OrdersCount}");
            }
            Console.WriteLine();
        }

        // Пайплайн 3: Продажі за місяцями та роками
        public static void SalesByYearAndMonth(List<Sale> sales, List<Product> products)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        s.SaleDate.Year,
                        s.SaleDate.Month,
                        s.Quantity,
                        Revenue = s.Quantity * p.UnitPrice
                    })
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalUnits = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 3: Продажі за місяцями та роками (Час: {sw.ElapsedTicks} ticks / {sw.Elapsed.TotalMilliseconds:F3} ms) ===");
            foreach (var item in result)
            {
                Console.WriteLine($"  {item.Year}-{item.Month:D2} | Продано одиниць: {item.TotalUnits,3} шт. | Дохід: {item.TotalRevenue,12:N2} грн");
            }
            Console.WriteLine();
        }

        // Пайплайн 4 (власний): Топ-5 VIP-клієнтів за 2026 рік (за сумою покупок > 15 000 грн)
        public static void TopVipCustomers2026(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Where(s => s.SaleDate.Year == 2026)
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        s.CustomerId,
                        p.Category,
                        Spent = s.Quantity * p.UnitPrice
                    })
                .Join(
                    customers,
                    sp => sp.CustomerId,
                    c => c.Id,
                    (sp, c) => new
                    {
                        c.Id,
                        c.Name,
                        c.City,
                        sp.Category,
                        sp.Spent
                    })
                .GroupBy(x => new { x.Id, x.Name, x.City })
                .Select(g => new
                {
                    g.Key.Name,
                    g.Key.City,
                    TotalSpent = g.Sum(x => x.Spent),
                    PurchasesCount = g.Count(),
                    Categories = string.Join(", ", g.Select(x => x.Category).Distinct())
                })
                .Where(x => x.TotalSpent > 15000m)
                .OrderByDescending(x => x.TotalSpent)
                .Take(5)
                .ToList();

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 4: Топ-5 VIP-клієнтів за 2026 рік (Час: {sw.ElapsedTicks} ticks / {sw.Elapsed.TotalMilliseconds:F3} ms) ===");
            foreach (var vip in result)
            {
                Console.WriteLine($"  {vip.Name} ({vip.City}) — {vip.TotalSpent:N2} грн ({vip.PurchasesCount} покупок) | Категорії: {vip.Categories}");
            }
            Console.WriteLine();
        }
    }
}
