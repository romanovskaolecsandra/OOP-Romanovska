using System;

namespace lab12v2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Демонстрація роботи InMemoryRepository ===");

            IRepository<User> repository = new InMemoryRepository<User>();

            // 1. Додавання
            repository.Add(new User { Id = 1, Username = "admin", Email = "admin@test.com", IsActive = true });
            repository.Add(new User { Id = 2, Username = "john_doe", Email = "john@test.com", IsActive = false });

            // 2. Отримання всіх
            Console.WriteLine("\nВсі користувачі:");
            foreach (var user in repository.GetAll())
            {
                Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Active: {user.IsActive}");
            }

            // 3. Оновлення
            var userToUpdate = repository.GetById(2);
            if (userToUpdate != null)
            {
                userToUpdate.IsActive = true;
                repository.Update(userToUpdate);
                Console.WriteLine($"\nКористувача з ID 2 оновлено (IsActive = true).");
            }

            // 4. Видалення
            repository.Delete(1);
            Console.WriteLine("\nКористувача з ID 1 видалено.");

            // Перевірка результату
            Console.WriteLine("\nЗалишилися в репозиторії:");
            foreach (var user in repository.GetAll())
            {
                Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Active: {user.IsActive}");
            }
        }
    }
}
