using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo
{
    /// <summary>
    /// Утиліти для нормалізації та токенізації тексту.
    /// </summary>
    public static class TextProcessor
    {
        private static readonly HashSet<string> StopWords = new()
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        /// <summary>
        /// Розбиває текст на слова, видаляє пунктуацію та переводить у нижній регістр.
        /// </summary>
        public static IEnumerable<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Enumerable.Empty<string>();

            var matches = Regex.Matches(text.ToLowerInvariant(), @"[\w#]+");
            return matches.Select(m => m.Value);
        }

        /// <summary>
        /// Видаляє загальновживані стоп-слова.
        /// </summary>
        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
        {
            return words.Where(w => !StopWords.Contains(w));
        }
    }
}

