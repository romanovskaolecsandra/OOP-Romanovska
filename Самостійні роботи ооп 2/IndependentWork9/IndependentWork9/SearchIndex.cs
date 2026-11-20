using System;
using System.Collections.Generic;

namespace IndependentWork9
{
    public class SearchIndex
    {
        // Слово -> Множина ID документів, де воно зустрічається
        private readonly Dictionary<string, HashSet<int>> _invertedIndex = new(StringComparer.OrdinalIgnoreCase);

        public void AddDocument(int docId, string content)
        {
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
            string[] words = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (!_invertedIndex.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _invertedIndex[word] = docIds;
                }
                docIds.Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            return _invertedIndex.TryGetValue(word, out var docIds)
                ? new HashSet<int>(docIds)
                : new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words.Length == 0)
            {
                return new HashSet<int>();
            }

            var result = Search(words[0]);
            for (int i = 1; i < words.Length; i++)
            {
                if (result.Count == 0) break;
                result.IntersectWith(Search(words[i]));
            }
            return result;
        }
    }
}
