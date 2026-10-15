using System;
using System.Collections.Generic;

namespace Lab11V2
{
    public abstract class DataImporter
    {
        public string FilePath { get; set; }
        protected bool Connected { get; set; }
        protected string RawText { get; set; }
        protected string ResultText { get; set; }

        protected DataImporter(string filePath)
        {
            FilePath = filePath;
            Connected = false;
        }

        public void Import()
        {
            Console.WriteLine("--- Початок обробки файлу: " + FilePath + " ---");
            Connect();
            
            if (Connected)
            {
                ReadData();
                if (Validate())
                {
                    Transform();
                    SaveToDatabase();
                }
                else
                {
                    Console.WriteLine("Помилка: валідація не пройдена.");
                }
                Disconnect();
            }
            Console.WriteLine("--- Обробка завершена ---\n");
        }

        protected abstract void ReadData();
        protected abstract void Transform();

        protected virtual void Connect()
        {
            Connected = true;
            Console.WriteLine("Підключення встановлено.");
        }

        protected virtual bool Validate()
        {
            Console.WriteLine("Стандартна валідація даних.");
            return !string.IsNullOrEmpty(RawText);
        }

        protected virtual void SaveToDatabase()
        {
            Console.WriteLine("Збереження в БД: " + ResultText);
        }

        protected virtual void Disconnect()
        {
            Connected = false;
            Console.WriteLine("Відключення виконано.");
        }
    }

    public class CSVImporter : DataImporter
    {
        public char Separator { get; set; }

        public CSVImporter(string filePath, char separator) : base(filePath)
        {
            Separator = separator;
        }

        protected override void ReadData()
        {
            Console.WriteLine("Зчитування CSV файлу з розділювачем: " + Separator);
            RawText = "1,Ivan,Admin";
        }

        protected override void Transform()
        {
            Console.WriteLine("Парсинг рядків CSV.");
            ResultText = "Користувач Ivan (роль Admin)";
        }

        protected override void Connect()
        {
            Console.WriteLine("Перевірка наявності CSV файлу...");
            base.Connect();
        }
    }

    public class JSONImporter : DataImporter
    {
        public bool StrictMode { get; set; }

        public JSONImporter(string filePath, bool strictMode) : base(filePath)
        {
            StrictMode = strictMode;
        }

        protected override void ReadData()
        {
            Console.WriteLine("Зчитування JSON. Режим Strict: " + StrictMode);
            RawText = "{id: 2, name: 'Olga'}";
        }

        protected override void Transform()
        {
            Console.WriteLine("Десеріалізація об'єкта JSON.");
            ResultText = "Користувач Olga з JSON файлу";
        }
    }

    public class XMLImporter : DataImporter
    {
        public string RootTag { get; set; }

        public XMLImporter(string filePath, string rootTag) : base(filePath)
        {
            RootTag = rootTag;
        }

        protected override void ReadData()
        {
            Console.WriteLine("Зчитування XML з тегом: " + RootTag);
            RawText = "<user><name>Petro</name></user>";
        }

        protected override void Transform()
        {
            Console.WriteLine("Конвертація XML дерева.");
            ResultText = "Користувач Petro з XML схеми";
        }

        protected override bool Validate()
        {
            Console.WriteLine("Перевірка структури тегів XML.");
            return base.Validate();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<DataImporter> list = new List<DataImporter>();

            list.Add(new CSVImporter("users.csv", ';'));
            list.Add(new JSONImporter("config.json", true));
            list.Add(new XMLImporter("data.xml", "catalog"));

            foreach (DataImporter importer in list)
            {
                importer.Import();
            }

            Console.ReadLine();
        }
    }
}
