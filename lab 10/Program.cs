using System;
using System.Collections.Generic;

namespace lab10v2
{
    public interface ISortable
    {
        void Sort(int[] array);
    }

    public class BubbleSort : ISortable
    {
        public void Sort(int[] array)
        {
            int temp;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    }

    public class QuickSort : ISortable
    {
        public void Sort(int[] array)
        {
            DoQuickSort(array, 0, array.Length - 1);
        }

        private void DoQuickSort(int[] array, int start, int end)
        {
            if (start >= end) return;

            int pivot = array[end];
            int i = start - 1;

            for (int j = start; j < end; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }

            int temp1 = array[i + 1];
            array[i + 1] = array[end];
            array[end] = temp1;

            int pivotIndex = i + 1;

            DoQuickSort(array, start, pivotIndex - 1);
            DoQuickSort(array, pivotIndex + 1, end);
        }
    }

    public abstract class LoggerBase
    {
        public abstract void WriteLog(string message);

        public void LogTimestamp(string message)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WriteLog($"[{time}] {message}");
        }
    }

    public class ConsoleLogger : LoggerBase
    {
        public override void WriteLog(string message)
        {
            Console.WriteLine("[ConsoleLog]: " + message);
        }
    }

    public class DebugLogger : LoggerBase
    {
        public override void WriteLog(string message)
        {
            Console.WriteLine("[DEBUG]: " + message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Перевірка роботи інтерфейсів (Сортування) ---");
            
            List<ISortable> sorters = new List<ISortable>();
            sorters.Add(new BubbleSort());
            sorters.Add(new QuickSort());

            foreach (var sorter in sorters)
            {
                int[] testArray = { 25, 4, 89, 12, 3, 44 };
                Console.WriteLine($"\nАлгоритм: {sorter.GetType().Name}");
                Console.WriteLine("До сортування: " + string.Join(", ", testArray));
                
                sorter.Sort(testArray);
                
                Console.WriteLine("Після сортування: " + string.Join(", ", testArray));
            }

            Console.WriteLine("\n--- Перевірка роботи абстрактних класів (Логування) ---");

            List<LoggerBase> loggers = new List<LoggerBase>();
            loggers.Add(new ConsoleLogger());
            loggers.Add(new DebugLogger());

            foreach (var logger in loggers)
            {
                Console.WriteLine($"\nТест класу: {logger.GetType().Name}");
                
                logger.WriteLog("Тестова подія відбулася успішно.");
                logger.LogTimestamp("Помилка з'єднання (тест).");
            }
        }
    }
}
