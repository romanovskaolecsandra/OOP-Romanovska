using System;
using System.Collections.Generic;
using System.Linq;

public class UserNotFoundException : Exception
{
    public string Username { get; }

    public UserNotFoundException(string username)
        : base($"Користувача з логіном '{username}' не знайдено.")
    {
        Username = username;
    }

    public UserNotFoundException(string username, string message)
        : base(message)
    {
        Username = username;
    }

    public UserNotFoundException(string username, string message, Exception innerException)
        : base(message, innerException)
    {
        Username = username;
    }
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(T value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, null);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default(T), false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return new Result<T>(default(T), false, ex.Message);
    }
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

public class UserService
{
    private readonly List<User> _users = new List<User>
    {
        new User("admin", "admin1234"),
        new User("student", "pass2026")
    };

    public User GetUser(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user == null)
        {
            throw new UserNotFoundException(username);
        }
        return user;
    }

    public Result<bool> RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result<bool>.Failure("Логін не може бути порожнім.");
        }

        if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<bool>.Failure($"Користувач із логіном '{username}' вже існує.");
        }

        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            return Result<bool>.Failure("Пароль занадто короткий (мінімум 6 символів).");
        }

        _users.Add(new User(username, password));
        return Result<bool>.Success(true);
    }
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var userService = new UserService();

        Console.WriteLine("=== 1. РЕЄСТРАЦІЯ КОРИСТУВАЧІВ (Патерн Result<T>) ===");

        var reg1 = userService.RegisterUser("admin", "newpass123");
        PrintRegistrationResult("admin", reg1);

        var reg2 = userService.RegisterUser("olena", "123");
        PrintRegistrationResult("olena", reg2);

        var reg3 = userService.RegisterUser("olena", "securePass2026");
        PrintRegistrationResult("olena", reg3);

        Console.WriteLine("\n=== 2. ПОШУК КОРИСТУВАЧІВ (Власний виняток UserNotFoundException) ===");

        try
        {
            var existingUser = userService.GetUser("olena");
            Console.WriteLine($"[Успіх] Знайдено користувача: {existingUser.Username}");

            Console.WriteLine("Спроба знайти неіснуючого користувача 'ghost'...");
            var missingUser = userService.GetUser("ghost");
            Console.WriteLine($"[Успіх] Знайдено користувача: {missingUser.Username}");
        }
        catch (UserNotFoundException ex)
        {
            Console.WriteLine($"[Помилка] Перехоплено власний виняток! Логін: '{ex.Username}'. Повідомлення: {ex.Message}");
        }
    }

    private static void PrintRegistrationResult(string username, Result<bool> result)
    {
        if (result.IsSuccess)
        {
            Console.WriteLine($"[Успіх] Користувача '{username}' успішно зареєстровано (Value = {result.Value}).");
        }
        else
        {
            Console.WriteLine($"[Відмова] Реєстрація '{username}' не вдалася: {result.Error}");
        }
    }
}
