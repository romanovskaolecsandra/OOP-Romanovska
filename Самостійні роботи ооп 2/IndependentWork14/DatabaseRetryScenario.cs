using System;
using System.Threading;
using Polly;
using Polly.Retry;

namespace IndependentWork14
{
    /// <summary>
    /// Сценарій 1: Тимчасова недоступність бази даних (Deadlock / Connection Timeout).
    /// Політика: Retry з експоненційною затримкою та Jitter.
    /// </summary>
    public static class DatabaseRetryScenario
    {
        private static int _dbAttempts = 0;

        private static string QueryDatabase(string sql)
        {
            _dbAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Спроба #{_dbAttempts}: Виконання SQL-запиту '{sql}'...");

            // Імітуємо 2 тимчасові збої з'єднання з БД
            if (_dbAttempts <= 2)
            {
                Thread.Sleep(150);
                throw new TimeoutException("Database connection pool timeout (Transient error).");
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Запит до БД виконано успішно!");
            return "{ Id: 101, Status: 'Paid', Amount: 2450.00 }";
        }

        public static void Run()
        {
            Console.WriteLine("=== Сценарій 1: Підключення до БД із політикою Retry (Exponential + Jitter) ===");

            var pipeline = new ResiliencePipelineBuilder<string>()
                .AddRetry(new RetryStrategyOptions<string>
                {
                    ShouldHandle = new PredicateBuilder<string>().Handle<TimeoutException>(),
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(400),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    OnRetry = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Retry] Повтор #{args.AttemptNumber + 1} через {args.RetryDelay.TotalMilliseconds:F0} мс. Причина: {args.Outcome.Exception?.Message}");
                        return default;
                    }
                })
                .Build();

            try
            {
                string data = pipeline.Execute(() => QueryDatabase("SELECT * FROM Orders WHERE Id = 101"));
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Отримані дані: {data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Усі спроби вичерпано: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}
