using System;
using IndependentWork7;

var cache = new Cache<int, string>(3);

Console.WriteLine("Додаємо 3 елементи (ємність кешу = 3)...");
cache.Add(1, "Перший");
cache.Add(2, "Другий");
cache.Add(3, "Третій");

Console.WriteLine($"Кількість елементів: {cache.Count}");
Console.WriteLine($"Отримання за ключем 2: {cache.Get(2)}");

Console.WriteLine("\nДодаємо 4-й елемент (ключ 1 має видалитися за принципом FIFO)...");
cache.Add(4, "Четвертий");

Console.WriteLine($"Кількість елементів: {cache.Count}");
Console.WriteLine($"Чи є ключ 1 у кеші? {cache.ContainsKey(1)}");
Console.WriteLine($"Чи є ключ 4 у кеші? {cache.ContainsKey(4)}");

try
{
    Console.WriteLine(cache.Get(1));
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Очікуваний виняток: {ex.Message}");
}
