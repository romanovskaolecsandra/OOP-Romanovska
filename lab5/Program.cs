using System;
using System.Collections.Generic;

namespace Lab5Variant2
{
    public class CustomStringList
    {
        private List<string> _items;

        public CustomStringList()
        {
            _items = new List<string>();
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public void Add(string item)
        {
            _items.Add(item);
        }

        public void Remove(string item)
        {
            _items.Remove(item);
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count)
                {
                    throw new IndexOutOfRangeException("Індекс вийшов за межі!");
                }
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _items.Count)
                {
                    throw new IndexOutOfRangeException("Індекс вийшов за межі!");
                }
                _items[index] = value;
            }
        }

        public static CustomStringList operator +(CustomStringList list1, CustomStringList list2)
        {
            CustomStringList result = new CustomStringList();
            
            for (int i = 0; i < list1.Count; i++)
            {
                result.Add(list1[i]);
            }
            for (int i = 0; i < list2.Count; i++)
            {
                result.Add(list2[i]);
            }
            
            return result;
        }

        public static bool operator ==(CustomStringList list1, CustomStringList list2)
        {
            if ((object)list1 == null && (object)list2 == null) return true;
            if ((object)list1 == null || (object)list2 == null) return false;
            
            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i]) return false;
            }
            return true;
        }

        public static bool operator !=(CustomStringList list1, CustomStringList list2)
        {
            return !(list1 == list2);
        }

        public override bool Equals(object obj)
        {
            CustomStringList other = obj as CustomStringList;
            if (other != null)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _items.GetHashCode();
        }

        public override string ToString()
        {
            string result = "[";
            for (int i = 0; i < _items.Count; i++)
            {
                result += _items[i];
                if (i < _items.Count - 1)
                {
                    result += ", ";
                }
            }
            result += "]";
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CustomStringList list1 = new CustomStringList();
            list1.Add("C#");
            list1.Add("Java");

            CustomStringList list2 = new CustomStringList();
            list2.Add("Python");
            list2.Add("C++");

            Console.WriteLine("Мій перший список: " + list1.ToString());
            Console.WriteLine("Мій другий список: " + list2.ToString());

            Console.WriteLine("\n--- Перевірка роботи індексатора ---");
            Console.WriteLine("Елемент під індексом 1 у першому списку: " + list1[1]);
            list1[1] = "JavaScript";
            Console.WriteLine("Змінений перший список: " + list1.ToString());

            Console.WriteLine("\n--- Перевірка оператора + ---");
            CustomStringList sumList = list1 + list2;
            Console.WriteLine("Результат об'єднання списков: " + sumList.ToString());

            Console.WriteLine("\n--- Перевірка операторів порівняння ---");
            CustomStringList list3 = new CustomStringList();
            list3.Add("C#");
            list3.Add("JavaScript");

            Console.WriteLine("Чи рівні список 1 та список 3? " + (list1 == list3));
            Console.WriteLine("Чи не рівні список 1 та список 2? " + (list1 != list2));
            
            Console.ReadLine();
        }
    }
}

