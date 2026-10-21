using System;
using System.Collections.Generic;
using System.Linq;

namespace lab12v2
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> _items = new();

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public void Add(T item)
        {
            if (_items.Any(x => x.Id == item.Id))
            {
                throw new ArgumentException($"Елемент з ID {item.Id} вже існує.");
            }
            _items.Add(item);
        }

        public void Update(T item)
        {
            var existing = GetById(item.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Елемент з ID {item.Id} не знайдено для оновлення.");
            }
            _items.Remove(existing);
            _items.Add(item);
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Елемент з ID {id} не знайдено для видалення.");
            }
            _items.Remove(existing);
        }
    }
}
