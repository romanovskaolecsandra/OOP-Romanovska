using System;
using System.IO;
using System.Net.Http;
using System.Threading;

public class FileProcessor
{
    private int _attempts = 0;

    public void WriteToFile(string path, string content)
    {
        _attempts++;
        if (_attempts <= 3)
        {
            throw new IOException($"Тимчасова помилка запису у файл '{path}' (спроба {_attempts}).");
        }
        Console.WriteLine($"[Успіх] Дані '{content}' успішно записано у файл '{path}'.");
    }
}

public class NetworkClient
{
    private int _attempts = 0;

    public bool UploadFile(string url, string filePath)
    {
        _attempts++;
        if (_attempts <= 2)
        {
            throw new HttpRequestException($"Тимчасова помилка мережі при завантаженні '{filePath}' на '{url}' (спроба {_attempts}).");
        }
        Console.WriteLine($"[Успіх] Файл '{filePath}' успішно завантажено на '{url}'.");
        return true;
    }
}

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default(TimeSpan),
        Func<Exception, bool> shouldRetry = null)
    {
        if (initialDelay == default(TimeSpan))
        {
            initialDelay = TimeSpan.FromMilliseconds(200);
        }

        int attempt = 0;
        while (true)
        {
            try
            {
                attempt++;
                Console.WriteLine($"-> Виконання операції (спроба {attempt})...");
                return operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Помилка] {ex.GetType().Name}: {ex.Message}");

                bool canRetry = attempt <= retryCount && (shouldRetry == null || shouldRetry(ex));
                if (!canRetry)
                {
                    Console.WriteLine("[Зупинка] Повторні спроби припинено. Виняток передано далі.");
                    throw;
                }

                double delayMs = initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
                TimeSpan currentDelay = TimeSpan.FromMilliseconds(delayMs);

                Console.WriteLine($"[Очікування] Пауза {currentDelay.TotalMilliseconds} мс перед наступною спробою...\n");
                Thread.Sleep(currentDelay);
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Func<Exception, bool> retryFilter = ex => ex is IOException || ex is HttpRequestException;

        Console.WriteLine("=== СЦЕНАРІЙ 1: Запис у файл (3 невдачі, на 4-й раз успіх) ===");
        var fileProcessor = new FileProcessor();
        RetryHelper.ExecuteWithRetry<bool>(
            () =>
            {
                fileProcessor.WriteToFile("report.txt", "Звіт за 2026 рік");
                return true;
            },
            retryCount: 4,
            initialDelay: TimeSpan.FromMilliseconds(300),
            shouldRetry: retryFilter
        );

        Console.WriteLine("\n=== СЦЕНАРІЙ 2: Завантаження файлу в мережу (2 невдачі, на 3-й раз успіх) ===");
        var networkClient = new NetworkClient();
        bool uploaded = RetryHelper.ExecuteWithRetry(
            () => networkClient.UploadFile("https://example.com", "report.txt"),
            retryCount: 3,
            initialDelay: TimeSpan.FromMilliseconds(300),
            shouldRetry: retryFilter
        );
        Console.WriteLine($"Результат завантаження: {uploaded}");

        Console.WriteLine("\n=== СЦЕНАРІЙ 3: Демонстрація вибіркової обробки (shouldRetry = false) ===");
        try
        {
            RetryHelper.ExecuteWithRetry<string>(
                () => throw new ArgumentException("Некоректний формат аргументу (повтор не має сенсу)."),
                retryCount: 3,
                initialDelay: TimeSpan.FromMilliseconds(200),
                shouldRetry: retryFilter
            );
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Перехоплено критичну помилку в Main без зайвих повторів: {ex.Message}");
        }
    }
}
