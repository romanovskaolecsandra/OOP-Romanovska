using System;
using System.Threading;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;

namespace IndependentWork14
{
    /// <summary>
    /// Сценарій 2: Каскадні збої мікросервісу оплати.
    /// Політика: Circuit Breaker (розмикач ланцюга) + Fallback (резервна відповідь).
    /// </summary>
    public static class PaymentCircuitBreakerScenario
    {
        private static string ProcessPayment(int orderId, bool serviceIsDown)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Запит до платіжного шлюзу для замовлення #{orderId}...");
            if (serviceIsDown)
            {
                throw new InvalidOperationException("Payment Gateway HTTP 503 Service Unavailable.");
            }

            return $"Оплата замовлення #{orderId} підтверджена миттєво!";
        }

        public static void Run()
        {
            Console.WriteLine("=== Сценарій 2: Мікросервіс оплати (Circuit Breaker + Fallback) ===");

            var pipeline = new ResiliencePipelineBuilder<string>()
                // Зовнішній рівень: Fallback повертає безпечну відповідь, якщо ланцюг розімкнено або сталася помилка
                .AddFallback(new FallbackStrategyOptions<string>
                {
                    ShouldHandle = new PredicateBuilder<string>()
                        .Handle<InvalidOperationException>()
                        .Handle<BrokenCircuitException>(),
                    FallbackAction = args => Outcome.FromResultAsValueTask("Платіж поставлено в чергу на обробку (Fallback)"),
                    OnFallback = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [Fallback] Активовано резервний сценарій через: {args.Outcome.Exception?.GetType().Name}");
                        return default;
                    }
                })
                // Внутрішній рівень: Circuit Breaker блокує запити після серії збоїв
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions<string>
                {
                    ShouldHandle = new PredicateBuilder<string>().Handle<InvalidOperationException>(),
                    FailureRatio = 0.5,
                    MinimumThroughput = 3,
                    SamplingDuration = TimeSpan.FromSeconds(10),
                    BreakDuration = TimeSpan.FromSeconds(2),
                    OnOpened = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [CircuitBreaker] Ланцюг РОЗІМКНЕНО (Open) на {args.BreakDuration.TotalSeconds} с! Запити блокуються.");
                        return default;
                    },
                    OnHalfOpened = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [CircuitBreaker] Ланцюг у стані НАПІВВІДКРИТИЙ (Half-Open): пробний запит...");
                        return default;
                    },
                    OnClosed = args =>
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [CircuitBreaker] Ланцюг ЗАМКНЕНО (Closed): роботу відновлено!");
                        return default;
                    }
                })
                .Build();

            // Імітуємо 4 невдалі запити підряд (на 3-му ланцюг розімкнеться, 4-й навіть не піде на сервер)
            for (int i = 1; i <= 4; i++)
            {
                string response = pipeline.Execute(() => ProcessPayment(i, serviceIsDown: true));
                Console.WriteLine($"  Результат виклику #{i}: {response}");
            }

            Console.WriteLine("Очікуємо 2.2 секунди, щоб Circuit Breaker перейшов у Half-Open...");
            Thread.Sleep(2200);

            // Сервіс відновився — пробний запит закриває ланцюг
            string recoveredResponse = pipeline.Execute(() => ProcessPayment(5, serviceIsDown: false));
            Console.WriteLine($"  Результат виклику #5: {recoveredResponse}\n");
        }
    }
}
