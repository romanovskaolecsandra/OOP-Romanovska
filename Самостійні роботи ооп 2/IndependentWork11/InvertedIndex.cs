using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo
{
    /// <summary>
    /// Інвертований індекс для швидкого повнотекстового пошуку документів.
    /// </summary>
    public class InvertedIndex
    {
        private readonly Dictionary<string, HashSet<int>> _index;
        private readonly Dictionary<int, Document> _documents;

        public InvertedIndex()
        {
            _index = new Dictionary<string, HashSet<int>>();
            _documents = new Dictionary<int, Document>();
        }

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Keys.Count;

        /// <summary>
        /// Додає документ до індексу.
        /// </summary>
        public void AddDocument(Document doc)
        {
            _documents[doc.Id] = doc;
            var tokens = TextProcessor.Tokenize(doc.Content);

            foreach (var word in tokens)
            {
                if (!_index.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _index[word] = docIds;
                }
                docIds.Add(doc.Id);
            }
        }

        /// <summary>
        /// Шукає документи, що містять вказане слово.
        /// </summary>
        public List<Document> SearchSingleWord(string word)
        {
            var normalizedWord = TextProcessor.Tokenize(word).FirstOrDefault();
            if (string.IsNullOrEmpty(normalizedWord) || !_index.TryGetValue(normalizedWord, out var docIds))
            {
                return new List<Document>();
            }

            return docIds.Select(id => _documents[id]).ToList();
        }

        /// <summary>
        /// Шукає документи, що містять ВСІ вказані слова (операція AND).
        /// </summary>
        public List<Document> SearchMultipleWords(params string[] words)
        {
            var normalizedWords = words
                .SelectMany(w => TextProcessor.Tokenize(w))
                .Distinct()
                .ToList();

            if (normalizedWords.Count == 0)
                return new List<Document>();

            if (!_index.TryGetValue(normalizedWords[0], out var firstSet))
                return new List<Document>();

            var resultIds = new HashSet<int>(firstSet);

            for (int i = 1; i < normalizedWords.Count; i++)
            {
                if (!_index.TryGetValue(normalizedWords[i], out var nextSet))
                {
                    return new List<Document>();
                }
                resultIds.IntersectWith(nextSet);
                if (resultIds.Count == 0)
                    break;
            }

            return resultIds.Select(id => _documents[id]).ToList();
        }

        /// <summary>
        /// Шукає документи, що містять БУДЬ-ЯКЕ зі вказаних слів (операція OR).
        /// </summary>
        public List<Document> SearchAnyWord(params string[] words)
        {
            var normalizedWords = words
                .SelectMany(w => TextProcessor.Tokenize(w))
                .Distinct()
                .ToList();

            var resultIds = new HashSet<int>();

            foreach (var word in normalizedWords)
            {
                if (_index.TryGetValue(word, out var docIds))
                {
                    resultIds.UnionWith(docIds);
                }
            }

            return resultIds.Select(id => _documents[id]).ToList();
        }
    }
}
