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
            Console.WriteLine($"[Block pamjati] Zjednannja stvoreno dlja bazy: {_connectionString}");
        }

        public void ExecuteQuery(string query)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DatabaseConnection), "Pomylka! Object uže znyščeno.");

            if (_isConnected)
                Console.WriteLine($"[Baza Danyh] Vykonuetsja zapyt: \"{query}\"");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("[Dispose(true)] Vydaljajemo kerovani resursy...");
                }

                if (_isConnected)
                {
                    Console.WriteLine("[Dispose] Zvilnjajemo nekerovanyj resurs: Zakryvajemo zjednannja.");
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
            Console.WriteLine("[Destructor] Systema sama čystyt object, bo pro njogo zabuly!");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== SCENARIJ 1: Avtomatyčne očyščennja čerez using ===");
            using (DatabaseConnection db1 = new DatabaseConnection("Server=MainDB;"))
            {
                db1.ExecuteQuery("SELECT * FROM Users");
            } 
            Console.WriteLine("Blok using zaveršyvsja. Object db1 vže znyščeno.\n");

            Console.WriteLine("=== SCENARIJ 2: Ručne očyščennja bez using ===");
            DatabaseConnection db2 = new DatabaseConnection("Server=BackupDB;");
            db2.ExecuteQuery("UPDATE Products SET Price = 100");
            db2.Dispose(); 
            Console.WriteLine("My vručnu vyklykaly Dispose(). Object db2 znyščeno.\n");

            Console.WriteLine("=== SCENARIJ 3: Object kynuly, čystyt zbyrač smittja ===");
            CreateObjectAndForget();
            
            Console.WriteLine("Prosymo systemu prymusovo prybraty smittja...");
            GC.Collect();
            GC.WaitForPendingFinalizers(); 
            
            Console.WriteLine("\nPrograma uspišno zaveršyla robotu.");
        }

        static void CreateObjectAndForget()
        {
            DatabaseConnection db3 = new DatabaseConnection("Server=TestDB;");
            db3.ExecuteQuery("INSERT INTO Logs VALUES ('Systemnyj test')");
        }
    }
}
