# Самостійна робота №11: Інвертований індекс і вимірювання продуктивності (`InvertedIndexDemo`)

## Опис проєкту
Консольний застосунок на C#, що реалізує інвертований індекс (`Dictionary<string, HashSet<int>>`) для повнотекстового пошуку (одне слово, логічне AND, логічне OR) та порівнює його швидкодію з лінійним пошуком. Детальний аналіз вимірювань наведено у файлі [REPORT.md](REPORT.md).

## Запуск
```bash
dotnet run
```

## Приклад виводу програми
```text
=== Тестування продуктивності (пошук слова 'async') ===

--- Обсяг: 10 документів ---
Linear search:  1842 ticks
Inverted Index: 138 ticks (прискорення у ~13.3x разів)
Перевірка: Linear found = 2, Index found = 2

--- Обсяг: 100 документів ---
Linear search:  14210 ticks
Inverted Index: 164 ticks (прискорення у ~86.6x разів)
Перевірка: Linear found = 20, Index found = 20

--- Обсяг: 1000 документів ---
Linear search:  138490 ticks
Inverted Index: 212 ticks (прискорення у ~653.3x разів)
Перевірка: Linear found = 200, Index found = 200

--- Обсяг: 10000 документів ---
Linear search:  1409820 ticks
Inverted Index: 379 ticks (прискорення у ~3719.8x разів)
Перевірка: Linear found = 2000, Index found = 2000

=== Інтерактивний пошук ===
Loaded 5 documents
Index contains 45 unique words

Enter search query (or 'exit'): programming language

--- Search Results (AND) ---
[1] Introduction to C#
  C# is a modern object-oriented programming language developed by Microsoft....
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
[3] Python Overview
  Python is a high-level programming language with simple syntax....
Found 3 documents (AND)

Enter search query (or 'exit'): c# async

--- Search Results (AND) ---
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....
Found 1 documents (AND)

--- Search Results (OR) ---
[1] Introduction to C#
[4] C# Advanced Features
[5] Comparing Languages
Found 3 documents (OR)

Enter search query (or 'exit'): exit
```
