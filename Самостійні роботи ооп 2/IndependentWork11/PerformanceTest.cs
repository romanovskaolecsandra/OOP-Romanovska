using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InvertedIndexDemo
{
    /// <summary>
    /// Клас для порівняння продуктивності лінійного пошуку та інвертованого індексу.
    /// </summary>
    public static class PerformanceTest
    {
        public static void RunBenchmarks(List<Document> baseDocuments, string searchWord)
        {
            int[] sizes = { 10, 100, 1000, 10000 };
            Console.WriteLine($"=== Тестування продуктивності (пошук слова '{searchWord}') ===");

            foreach (int size in sizes)
            {
                var docs = GenerateDocuments(baseDocuments, size);
                Console.WriteLine($"\n--- Обсяг: {size} документів ---");
                CompareSearchMethods(docs, searchWord);
            }
            Console.WriteLine();
        }

        public static void CompareSearchMethods(List<Document> documents, string searchWord)
        {
            var sw = new Stopwatch();

            // 1. Лінійний пошук (однакова нормалізація через TextProcessor.Tokenize)
            sw.Start();
            var normalizedWord = TextProcessor.Tokenize(searchWord).FirstOrDefault() ?? "";
            var linearResults = documents
                .Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord))
                .ToList();
            sw.Stop();
            long linearTicks = sw.ElapsedTicks;

            // 2. Побудова індексу та пошук через InvertedIndex
            var index = new InvertedIndex();
            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }

            sw.Restart();
            var indexResults = index.SearchSingleWord(searchWord);
            sw.Stop();
            long indexTicks = sw.ElapsedTicks;

            double speedup = indexTicks > 0 ? Math.Round((double)linearTicks / indexTicks, 1) : 0;

            Console.WriteLine($"Linear search:  {linearTicks} ticks");
            Console.WriteLine($"Inverted Index: {indexTicks} ticks (прискорення у ~{speedup}x разів)");
            Console.WriteLine($"Перевірка: Linear found = {linearResults.Count}, Index found = {indexResults.Count}");
        }

        private static List<Document> GenerateDocuments(List<Document> templates, int count)
        {
            var result = new List<Document>(count);
            var random = new Random(42);
            string[] extraWords = { "database", "algorithm", "cloud", "network", "security", "compiler", "memory", "thread" };

            for (int i = 1; i <= count; i++)
            {
                var template = templates[(i - 1) % templates.Count];
                string extra = $"{extraWords[random.Next(extraWords.Length)]} {extraWords[random.Next(extraWords.Length)]}";
                result.Add(new Document
                {
                    Id = i,
                    Title = $"{template.Title} #{i}",
                    Content = $"{template.Content} {extra}"
                });
            }
            return result;
        }
    }
}
