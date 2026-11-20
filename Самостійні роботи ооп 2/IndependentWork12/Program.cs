using System;
using IndependentWork12;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var (products, customers, sales) = DataGenerator.GenerateData();

Console.WriteLine($"Згенеровано тестові дані: {products.Count} продуктів, {customers.Count} клієнтів, {sales.Count} продажів.\n");

AnalyticsPipelines.TopProductsByCategory(sales, products);
AnalyticsPipelines.TotalRevenueByCity(sales, products, customers);
AnalyticsPipelines.SalesByYearAndMonth(sales, products);
AnalyticsPipelines.TopVipCustomers2026(sales, products, customers);
