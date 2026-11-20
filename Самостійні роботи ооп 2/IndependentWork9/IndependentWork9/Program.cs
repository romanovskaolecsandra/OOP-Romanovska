using System;
using IndependentWork9;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Сценарій 1: Кеш метаданих файлів (Dictionary) ===");
var fileCache = new FileMetadataCache();
fileCache.Add(@"C:\Docs\report.pdf", new FileMetadata
{
    FileName = "report.pdf",
    Size = 245760,
    LastModified = DateTime.Now,
    Hash = "A1B2C3D4"
});
fileCache.Add(@"C:\Docs\data.json", new FileMetadata
{
    FileName = "data.json",
    Size = 1024,
    LastModified = DateTime.Now,
    Hash = "E5F6G7H8"
});

Console.WriteLine($"Чи є report.pdf (перевірка без урахування регістру): {fileCache.Contains(@"c:\docs\REPORT.PDF")}");
Console.WriteLine($"Отримано: {fileCache.Get(@"C:\Docs\report.pdf")}");
fileCache.Remove(@"C:\Docs\data.json");
Console.WriteLine($"Кількість файлів у кеші після видалення: {fileCache.Count}\n");

Console.WriteLine("=== Сценарій 2: Унікальні теги для статей (HashSet) ===");
var tagManager = new TagManager();
tagManager.AddTag("article-1", "C#");
tagManager.AddTag("article-1", "DOTNET");
tagManager.AddTag("article-1", "c#"); // Дублікат, не додасться
tagManager.AddTag("article-1", "Collections");

tagManager.AddTag("article-2", "dotnet");
tagManager.AddTag("article-2", "Performance");
tagManager.AddTag("article-2", "Collections");

Console.WriteLine($"Усі унікальні теги: {string.Join(", ", tagManager.GetAllUniqueTags())}");
Console.WriteLine($"Спільні теги article-1 та article-2: {string.Join(", ", tagManager.FindCommonTags("article-1", "article-2"))}\n");

Console.WriteLine("=== Сценарій 3: Інвертований індекс пошуку (Dictionary + HashSet) ===");
var searchIndex = new SearchIndex();
searchIndex.AddDocument(1, "C# і .NET дозволяють створювати швидкі консольні застосунки.");
searchIndex.AddDocument(2, "Колекції Dictionary та HashSet у .NET мають складність пошуку O(1).");
searchIndex.AddDocument(3, "Словник Dictionary зберігає пари ключ-значення у C#.");

Console.WriteLine($"Документи зі словом 'Dictionary': [{string.Join(", ", searchIndex.Search("Dictionary"))}]");
Console.WriteLine($"Документи зі словами 'C#' та 'Dictionary': [{string.Join(", ", searchIndex.SearchMultiple("C#", "Dictionary"))}]");
