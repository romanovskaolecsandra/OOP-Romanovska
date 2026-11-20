using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IndependentWork13
{
    public static class ThreadSafetyDemo
    {
        public static void Run()
        {
            Console.WriteLine("================ Дослідження безпеки та побічних ефектів у PLINQ ================");
            const int itemsCount = 100_000;
            var numbers = Enumerable.Range(1, itemsCount).ToList();

            Console.WriteLine($"Очікувана кількість парних чисел у діапазоні 1..{itemsCount:N0}: {itemsCount / 2:N0}\n");

            // 1. НЕБЕЗПЕЧНИЙ ВАРІАНТ (стан гонитви / Race Condition)
            Console.WriteLine("--- 1. Небезпечна модифікація спільної змінної без синхронізації (3 прогони) ---");
            for (int run = 1; run <= 3; run++)
            {
                int unsafeCounter = 0;
                var unsafeList = new List<int>();

                try
                {
                    numbers.AsParallel().ForAll(n =>
                    {
                        if (n % 2 == 0)
                        {
                            // Неатомарна операція (читання -> збільшення -> запис) призводить до втрати інкрементів
                            unsafeCounter++;
                            // List<T>.Add не є потокобезпечним і може пошкодити внутрішній масив або кинути виняток
                            unsafeList.Add(n);
                        }
                    });
                    Console.WriteLine($"  Прогін {run}: unsafeCounter = {unsafeCounter:N0} (втрачено {50000 - unsafeCounter:N0}), unsafeList.Count = {unsafeList.Count:N0}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Прогін {run}: Виник виняток через пошкодження List<T> -> {ex.GetType().Name}: {ex.Message}");
                }
            }

            // 2. БЕЗПЕЧНИЙ ВАРІАНТ 1: Використання lock
            Console.WriteLine("\n--- 2. Виправлення через оператор lock ---");
            int lockCounter = 0;
            var safeListWithLock = new List<int>();
            object syncRoot = new object();

            numbers.AsParallel().ForAll(n =>
            {
                if (n % 2 == 0)
                {
                    lock (syncRoot)
                    {
                        lockCounter++;
                        safeListWithLock.Add(n);
                    }
                }
            });
            Console.WriteLine($"  Результат із lock: counter = {lockCounter:N0}, list.Count = {safeListWithLock.Count:N0} (Коректно!)");

            // 3. БЕЗПЕЧНИЙ ВАРІАНТ 2: Interlocked та потокобезпечна колекція ConcurrentBag<T> (без блокувань)
            Console.WriteLine("\n--- 3. Виправлення через Interlocked та ConcurrentBag<T> ---");
            int interlockedCounter = 0;
            var concurrentBag = new ConcurrentBag<int>();

            numbers.AsParallel().ForAll(n =>
            {
                if (n % 2 == 0)
                {
                    Interlocked.Increment(ref interlockedCounter);
                    concurrentBag.Add(n);
                }
            });
            Console.WriteLine($"  Результат із Interlocked + ConcurrentBag: counter = {interlockedCounter:N0}, bag.Count = {concurrentBag.Count:N0} (Коректно!)");

            // 4. НАЙКРАЩА ПРАКТИКА PLINQ: Декларативний стиль без побічних ефектів
            Console.WriteLine("\n--- 4. Ідіоматичний PLINQ без побічних ефектів (.Where().ToList()) ---");
            var cleanResult = numbers
                .AsParallel()
                .Where(n => n % 2 == 0)
                .ToList();
            Console.WriteLine($"  Результат декларативного запиту: count = {cleanResult.Count:N0} (Коректно та найшвидше!)");
        }
    }
}
