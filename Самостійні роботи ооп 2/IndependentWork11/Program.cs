using System;
using System.Collections.Generic;
using InvertedIndexDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var baseDocuments = new List<Document>
{
    new Document
    {
        Id = 1,
        Title = "Introduction to C#",
        Content = "C# is a modern object-oriented programming language developed by Microsoft."
    },
    new Document
    {
        Id = 2,
        Title = "Java Basics",
        Content = "Java is a popular programming language known for its portability and performance."
    },
    new Document
    {
        Id = 3,
        Title = "Python Overview",
        Content = "Python is a high-level programming language with simple syntax."
    },
    new Document
    {
        Id = 4,
        Title = "C# Advanced Features",
        Content = "C# supports LINQ, async/await, and many modern programming paradigms."
    },
    new Document
    {
        Id = 5,
        Title = "Comparing Languages",
        Content = "C#, Java, and Python are among the most popular programming languages today."
    }
};

// 1. Запуск бенчмарків для 10 / 100 / 1000 / 10000 документів
PerformanceTest.RunBenchmarks(baseDocuments, "async");

// 2. Інтерактивна демонстрація пошуку
var index = new InvertedIndex();
foreach (var doc in baseDocuments)
{
    index.AddDocument(doc);
}

Console.WriteLine("=== Інтерактивний пошук ===");
Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words\n");

while (true)
{
    Console.Write("Enter search query (or 'exit'): ");
    var query = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(query) || query.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    Console.WriteLine("\n--- Search Results (AND) ---");
    var andResults = index.SearchMultipleWords(words);
    foreach (var doc in andResults)
    {
        Console.WriteLine(doc);
        Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
    }
    Console.WriteLine($"Found {andResults.Count} documents (AND)");

    if (words.Length > 1)
    {
        Console.WriteLine("\n--- Search Results (OR) ---");
        var orResults = index.SearchAnyWord(words);
        foreach (var doc in orResults)
        {
            Console.WriteLine(doc);
        }
        Console.WriteLine($"Found {orResults.Count} documents (OR)");
    }
    Console.WriteLine();
}
