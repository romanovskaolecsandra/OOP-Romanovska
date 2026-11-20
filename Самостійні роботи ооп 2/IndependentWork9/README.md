# Самостійна робота №9: Вивчення `Dictionary<TKey, TValue>` та `HashSet<T>`

## Опис
Консольний застосунок на C#, що демонструє практичні сценарії використання `Dictionary<TKey, TValue>` та `HashSet<T>`, а також звіт з аналізом open-source бібліотек у файлі [REPORT.md](REPORT.md).

## Запуск
```bash
dotnet run
```

## Приклад виводу програми
```text
=== Сценарій 1: Кеш метаданих файлів (Dictionary) ===
Чи є report.pdf (перевірка без урахування регістру): True
Отримано: report.pdf (245760 байт, змінено: 2026-11-20 16:15, Hash: A1B2C3D4)
Кількість файлів у кеші після видалення: 1

=== Сценарій 2: Унікальні теги для статей (HashSet) ===
Усі унікальні теги: C#, DOTNET, Collections, Performance
Спільні теги article-1 та article-2: DOTNET, Collections

=== Сценарій 3: Інвертований індекс пошуку (Dictionary + HashSet) ===
Документи зі словом 'Dictionary': [2, 3]
Документи зі словами 'C#' та 'Dictionary': [3]
```
