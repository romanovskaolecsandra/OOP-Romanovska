using System;
using System.Threading;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace IndependentWork14
{
    /// <summary>
    /// Сценарій 3: Зовнішній сервіс аналітики занадто довго формує звіт.
    /// Політика: Комбінація Retry + Timeout (обмеження часу очікування на кожну спробу).
    /// </summary>
    public static class ReportTimeoutScenario
    {
        private static int _reportAttempts = 0;

        private static string GenerateExternalReport(CancellationToken token)
        {
            _reportAttempts++;
            int workDurationMs = _reportAttempts == 1 ? 1200 : 200; // 1-ша спроба "зависає" на 1.2с, 2-га швидка (0.2с)
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Спроба #{_reportAttempts}: Генерація звіту (очікуваний час {workDurationMs} мс)...");

            // Кооперативне очікування з підтримкою скасування через Timeout
            if (token.WaitHandle.WaitOne(workDurationMs))
            {
                token.ThrowIfCancellationRequested();
            }
            token.ThrowIfCancellationRequested();

            return "PDF_Report_2026_Final.pdf (1.4 MB)";
        }

        public static void Run()
        {
            Console.WriteLine("=== Сценарій 3: Генерація звіту (Комбінація Retry + Timeout) ===");

            var pipeline = new ResiliencePipelineBuilder<string>()
                .AddRetry(new RetryStrategyOptions<string>
                {
                    ShouldHandle = new PredicateBuilder<string>().Handle<TimeoutRejectedException>(),
                    MaxRetryAttempts = 2,
                    Delay = TimeSpan.FromMilliseconds(200),
                    OnRetry = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Retry] Повторна спроба після перевищення ліміту часу!");
                        return default;
                    }
                })
                .AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromMilliseconds(500), // Максимум 500 мс на одну спробу
                    OnTimeout = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Timeout] Операцію перервано: перевищено ліміт {args.Timeout.TotalMilliseconds} мс!");
                        return default;
                    }
                })
                .Build();

            string report = pipeline.Execute(token => GenerateExternalReport(token));
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Фінальний результат: {report}\n");
        }
    }
}
