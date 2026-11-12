using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

public class OrderDetail
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }

    public OrderDetail(int id, int productId, int quantity, Product product)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        Product = product;
    }
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetail> Details { get; set; }

    public Order(int id, DateTime orderDate, List<OrderDetail> details)
    {
        Id = id;
        OrderDate = orderDate;
        Details = details;
    }
}

public static class OrderExtensions
{
    public static decimal TotalOrderAmount(this IEnumerable<OrderDetail> details)
    {
        return details.Sum(d => d.Quantity * d.Product.Price);
    }
}

public class Program
{
    private static OrderDetail CreateDetail(List<Product> products, int id, int productId, int quantity)
    {
        var product = products.First(p => p.Id == productId);
        return new OrderDetail(id, productId, quantity, product);
    }

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var products = new List<Product>
        {
            new Product(1, "Ноутбук", 32000m),
            new Product(2, "Мишка бездротова", 850m),
            new Product(3, "Клавіатура механічна", 2400m),
            new Product(4, "Монітор 27\"", 11500m),
            new Product(5, "Навушники", 1900m),
            new Product(6, "Веб-камера", 1500m),
            new Product(7, "Килимок для миші", 400m),
            new Product(8, "USB-хаб", 650m),
            new Product(9, "Зовнішній SSD 1TB", 3800m),
            new Product(10, "Підставка під ноутбук", 950m)
        };

        var orders = new List<Order>
        {
            new Order(101, new DateTime(2026, 11, 1), new List<OrderDetail>
            {
                CreateDetail(products, 1, 1, 1),
                CreateDetail(products, 2, 2, 1),
                CreateDetail(products, 3, 7, 1)
            }),
            new Order(102, new DateTime(2026, 11, 2), new List<OrderDetail>
            {
                CreateDetail(products, 4, 3, 1),
                CreateDetail(products, 5, 2, 2)
            }),
            new Order(103, new DateTime(2026, 11, 3), new List<OrderDetail>
            {
                CreateDetail(products, 6, 4, 2),
                CreateDetail(products, 7, 8, 1),
                CreateDetail(products, 8, 5, 1)
            }),
            new Order(104, new DateTime(2026, 11, 4), new List<OrderDetail>
            {
                CreateDetail(products, 9, 9, 1),
                CreateDetail(products, 10, 1, 1)
            }),
            new Order(105, new DateTime(2026, 11, 5), new List<OrderDetail>
            {
                CreateDetail(products, 11, 6, 1),
                CreateDetail(products, 12, 5, 2),
                CreateDetail(products, 13, 10, 1)
            }),
            new Order(106, new DateTime(2026, 11, 6), new List<OrderDetail>
            {
                CreateDetail(products, 14, 7, 3),
                CreateDetail(products, 15, 3, 1)
            })
        };

        Console.WriteLine("=== 1. УСІ ЗАМОВЛЕНІ ПРОДУКТИ ===");

        var orderedProducts = orders
            .SelectMany(o => o.Details)
            .Select(d => d.Product)
            .Distinct()
            .ToList();

        foreach (var product in orderedProducts)
        {
            Console.WriteLine($"- [{product.Id}] {product.Name} ({product.Price} грн)");
        }

        Console.WriteLine("\n=== 2. ПАГІНАЦІЯ ЗАМОВЛЕНЬ ===");

        int pageSize = 2;
        int totalPages = (int)Math.Ceiling((double)orders.Count / pageSize);

        for (int page = 1; page <= totalPages; page++)
        {
            Console.WriteLine($"\n--- Сторінка {page} з {totalPages} ---");
            var pageOrders = orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            foreach (var order in pageOrders)
            {
                Console.WriteLine($"Замовлення №{order.Id} від {order.OrderDate:dd.MM.yyyy} (позицій: {order.Details.Count})");
            }
        }

        Console.WriteLine("\n=== 3. ЗАГАЛЬНА СУМА КОЖНОГО ЗАМОВЛЕННЯ ===");

        foreach (var order in orders)
        {
            decimal totalAmount = order.Details.TotalOrderAmount();
            Console.WriteLine($"Замовлення №{order.Id}: сума = {totalAmount} грн");
        }
    }
}
