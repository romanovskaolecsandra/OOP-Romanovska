using System;
using System.Threading;

namespace IndependentWork15
{
    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _initialDelay;
        private readonly double _jitterFactor;
        private readonly Random _random = new();

        public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
        {
            if (maxRetries < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "Кількість спроб не може бути від'ємною.");
            if (initialDelay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(initialDelay), "Початкова затримка не може бути від'ємною.");
            if (jitterFactor < 0 || jitterFactor > 1)
                throw new ArgumentOutOfRangeException(nameof(jitterFactor), "Коефіцієнт jitter має бути в межах [0..1].");

            _maxRetries = maxRetries;
            _initialDelay = initialDelay;
            _jitterFactor = jitterFactor;
        }

        public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
        {
            ArgumentNullException.ThrowIfNull(operation);

            int retryAttempt = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Виконання операції (спроба #{retryAttempt + 1})...");
                    return operation();
                }
                catch (Exception ex)
                {
                    // Перевіряємо, чи дозволено повторювати цей тип винятку
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Abort] Виняток {ex.GetType().Name} не підлягає повтору (shouldRetry == false).");
                        throw;
                    }

                    // Якщо вичерпано ліміт повторних спроб — прокидаємо виняток далі
                    if (retryAttempt >= _maxRetries)
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Failed] Вичерпано всі {_maxRetries} повторних спроб. Остання помилка: {ex.Message}");
                        throw;
                    }

                    // Формула з методички: exponential backoff + випадковий jitter
                    TimeSpan delay = _initialDelay * Math.Pow(2, retryAttempt) * (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);

                    retryAttempt++;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Retry #{retryAttempt}/{_maxRetries}] Помилка ({ex.GetType().Name}: {ex.Message}). Очікування {delay.TotalMilliseconds:F0} мс...");

                    Thread.Sleep(delay);
                }
            }
        }
    }
}
