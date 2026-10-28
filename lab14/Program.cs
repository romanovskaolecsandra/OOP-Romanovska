using System;
using System.Collections.Generic;

namespace LogProcessingApp
{
    public class LogEntry
    {
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public LogEntry(string ipAddress, string message)
        {
            IpAddress = ipAddress;
            Timestamp = DateTime.Now;
            Message = message;
        }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] IP: {IpAddress} | {Message}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №14 | ВАРІАНТ 2: ОБРОБКА ЛОГІВ ===\n");

            HashSet<string> uniqueIps = new HashSet<string>();
            Queue<LogEntry> logQueue = new Queue<LogEntry>();
            Stack<LogEntry> processingHistory = new Stack<LogEntry>();

            Console.WriteLine("--- Крок 1: Надходження нових логів у чергу (Queue.Enqueue) ---");
            
            logQueue.Enqueue(new LogEntry("192.168.1.10", "Користувач увійшов у систему"));
            logQueue.Enqueue(new LogEntry("10.0.0.5", "Завантажено файл report.pdf"));
            logQueue.Enqueue(new LogEntry("192.168.1.10", "Зміна паролю (Той самий IP)"));
            logQueue.Enqueue(new LogEntry("172.16.0.1", "Спроба підключення до бази даних"));

            Console.WriteLine($"Додано логів у чергу: {logQueue.Count}");
            Console.WriteLine($"Наступний лог, що очікує на обробку (Queue.Peek):\n  {logQueue.Peek()}\n");

            Console.WriteLine("--- Крок 2: Обробка черги та збір унікальних IP-адрес (HashSet) ---");
            
            while (logQueue.Count > 0)
            {
                LogEntry currentLog = logQueue.Dequeue();
                Console.WriteLine($"[Черга -> Обробка]: {currentLog}");

                bool isAdded = uniqueIps.Add(currentLog.IpAddress);
                Console.WriteLine($"   * Додавання IP {currentLog.IpAddress} до HashSet. Результат: {(isAdded ? "Унікальний (Додано)" : "Дублікат (Пропущено)")}");

                processingHistory.Push(currentLog);
            }
            Console.WriteLine();

            Console.WriteLine("--- Результати роботи HashSet ---");
            Console.WriteLine($"Усього знайдено унікальних IP: {uniqueIps.Count}");
            foreach (var ip in uniqueIps)
            {
                Console.WriteLine($" - IP: {ip}");
            }
            Console.WriteLine($"Перевірка наявності IP '10.0.0.5' в базі: {uniqueIps.Contains("10.0.0.5")}\n");

            Console.WriteLine("--- Крок 3: Перегляд історії обробки у зворотному порядку (Stack.Pop) ---");
            Console.WriteLine($"Останній оброблений лог на вершині стеку (Stack.Peek):\n  {processingHistory.Peek()}\n");

            Console.WriteLine("Вилучення логів зі стеку історії (від останнього до найпершого):");
            while (processingHistory.Count > 0)
            {
                LogEntry historyLog = processingHistory.Pop();
                Console.WriteLine($" [Стек -> Історія]: {historyLog}");
            }

            Console.WriteLine("\nПрограму успішно завершено.");
        }
    }
}
