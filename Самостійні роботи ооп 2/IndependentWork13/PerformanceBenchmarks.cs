using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork13
{
    public static class PerformanceBenchmarks
    {
        public static void RunAll()
        {
            Console.WriteLine($"Кількість логічних ядер процесора: {Environment.ProcessorCount}\n");

            int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };

            foreach (int size in sizes)
            {
                Console.WriteLine($"================ Розмір колекції: {size:N0} елементів ================");
                var data = GenerateData(size);

                // Експеримент 1: Перевірка простих чисел (високе CPU-навантаження)
                ComparePrimeSearch(data);

                // Експеримент 2: Складні математичні перетворення
                CompareMathTransform(data);

                Console.WriteLine();
            }
        }

        private static List<int> GenerateData(int count)
        {
            var random = new Random(42);
            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(random.Next(1, 500_000));
            }
            return list;
        }

        private static void ComparePrimeSearch(List<int> data)
        {
            // 1. Звичайний LINQ
            var swLinq = Stopwatch.StartNew();
            var linqResult = data
                .Where(n => MathOperations.IsPrime(n))
                .Select(n => (long)n * 2)
                .ToList();
            swLinq.Stop();

            // 2. PLINQ (AsParallel)
            var swPlinq = Stopwatch.StartNew();
            var plinqResult = data
                .AsParallel()
                .Where(n => MathOperations.IsPrime(n))
                .Select(n => (long)n * 2)
                .ToList();
            swPlinq.Stop();

            double speedup = (double)swLinq.ElapsedMilliseconds / Math.Max(1, swPlinq.ElapsedMilliseconds);
            Console.WriteLine($"[Операція 1: Пошук простих чисел]");
            Console.WriteLine($"  LINQ:  {swLinq.ElapsedMilliseconds,5} ms (знайдено: {linqResult.Count:N0})");
            Console.WriteLine($"  PLINQ: {swPlinq.ElapsedMilliseconds,5} ms (знайдено: {plinqResult.Count:N0}) -> Прискорення: {speedup:F2}x");
        }

        private static void CompareMathTransform(List<int> data)
        {
            // 1. Звичайний LINQ
            var swLinq = Stopwatch.StartNew();
            var linqResult = data
                .Where(n => n % 3 == 0)
                .Select(n => MathOperations.HeavyMathTransform(n))
                .ToList();
            swLinq.Stop();

            // 2. PLINQ (AsParallel)
            var swPlinq = Stopwatch.StartNew();
            var plinqResult = data
                .AsParallel()
                .Where(n => n % 3 == 0)
                .Select(n => MathOperations.HeavyMathTransform(n))
                .ToList();
            swPlinq.Stop();

            double speedup = (double)swLinq.ElapsedMilliseconds / Math.Max(1, swPlinq.ElapsedMilliseconds);
            Console.WriteLine($"[Операція 2: Математичні обчислення (Sqrt/Sin/Log)]");
            Console.WriteLine($"  LINQ:  {swLinq.ElapsedMilliseconds,5} ms (елементів: {linqResult.Count:N0})");
            Console.WriteLine($"  PLINQ: {swPlinq.ElapsedMilliseconds,5} ms (елементів: {plinqResult.Count:N0}) -> Прискорення: {speedup:F2}x");
        }
    }
}
