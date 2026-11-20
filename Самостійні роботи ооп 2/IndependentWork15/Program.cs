using System;
using System.Net.Http;
using IndependentWork15;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Створюємо політику: максимум 3 повторні спроби, початкова затримка 400 мс, jitter ±25%
var retryPolicy = new RetryPolicy(maxRetries: 3, initialDelay: TimeSpan.FromMilliseconds(400), jitterFactor: 0.25);

// Фільтр: повторюємо лише тимчасові мережеві помилки або таймаути
Func<Exception, bool> isTransientError = ex => ex is HttpRequestException || ex is TimeoutException;

// ================= СЦЕНАРІЙ 1: Тимчасовий збій -> Успіх =================
Console.WriteLine("=== Сценарій 1: Операція тимчасово збоїть, але на 3-й спробі проходить успішно ===");
int attemptCounter1 = 0;
try
{
    string result = retryPolicy.Execute(() =>
    {
        attemptCounter1++;
        if (attemptCounter1 <= 2)
        {
            throw new HttpRequestException("Сервер тимчасово перевантажений (503).");
        }
        return "Дані профілю користувача успішно завантажено!";
    }, isTransientError);

    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Результат: {result}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"Неочікувана помилка: {ex.Message}\n");
}

// ================= СЦЕНАРІЙ 2: Постійна відмова -> Виняток =================
Console.WriteLine("=== Сценарій 2: Операція завжди падає -> вичерпання всіх спроб і виняток ===");
try
{
    retryPolicy.Execute<int>(() =>
    {
        throw new TimeoutException("Перевищено час очікування відповіді від БД.");
    }, isTransientError);
}
catch (Exception ex)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Спіймано фінальний виняток у Main: {ex.GetType().Name} — {ex.Message}\n");
}

// ================= СЦЕНАРІЙ 3: Критична помилка (shouldRetry == false) =================
Console.WriteLine("=== Сценарій 3: Невиправна помилка (shouldRetry повертає false -> без повторів) ===");
try
{
    retryPolicy.Execute<string>(() =>
    {
        throw new UnauthorizedAccessException("Невірний API-ключ доступу (401).");
    }, isTransientError);
}
catch (Exception ex)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Операцію зупинено одразу: {ex.GetType().Name} — {ex.Message}\n");
}
