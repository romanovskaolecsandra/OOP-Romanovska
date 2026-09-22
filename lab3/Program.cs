using System;

namespace Lab3
{
    public class DatabaseConnection : IDisposable
    {
        private bool _disposed = false;
        private string _connectionString;
        private bool _isConnected; 

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
            _isConnected = true; 
            Console.WriteLine($"[Блок пам'яті] З'єднання створено для бази: {_connectionString}");
        }

        public void ExecuteQuery(string query)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DatabaseConnection), "Помилка! Об'єкт уже знищено.");

            if (_isConnected)
                Console.WriteLine($"[База Даних] Виконується запит: \"{query}\"");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("[Dispose(true)] Видаляємо керовані ресурси...");
                }

                if (_isConnected)
                {
                    Console.WriteLine("[Dispose] Звільняємо некерований ресурс: Закриваємо з'єднання.");
                    _isConnected = false;
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        ~DatabaseConnection()
        {
            Console.WriteLine("[Деструктор] Система сама чистить об'єкт, бо про нього забули!");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== СЦЕНАРІЙ 1: Автоматичне очищення через using ===");
            using (DatabaseConnection db1 = new DatabaseConnection("Server=MainDB;"))
            {
                db1.ExecuteQuery("SELECT * FROM Users");
            } 
            Console.WriteLine("Блок using завершився. Об'єкт db1 вже знищено.\n");

            Console.WriteLine("=== СЦЕНАРІЙ 2: Ручне очищення без using ===");
            DatabaseConnection db2 = new DatabaseConnection("Server=BackupDB;");
            db2.ExecuteQuery("UPDATE Products SET Price = 100");
            db2.Dispose(); 
            Console.WriteLine("Ми вручну викликали Dispose(). Об'єкт db2 знищено.\n");

            Console.WriteLine("=== СЦЕНАРІЙ 3: Об'єкт кинули, чистить збирач сміття ===");
            CreateObjectAndForget();
            
            Console.WriteLine("Просимо систему примусово прибрати сміття...");
            GC.Collect();
            GC.WaitForPendingFinalizers(); 
            
            Console.WriteLine("\nПрограма успішно завершила роботу.");
        }

        static void CreateObjectAndForget()
        {
            DatabaseConnection db3 = new DatabaseConnection("Server=TestDB;");
            db3.ExecuteQuery("INSERT INTO Logs VALUES ('Системний тест')");
        }
    }
}
