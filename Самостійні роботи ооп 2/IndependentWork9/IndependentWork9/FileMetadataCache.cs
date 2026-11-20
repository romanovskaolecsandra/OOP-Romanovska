using System;
using System.Collections.Generic;

namespace IndependentWork9
{
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; } = string.Empty;

        public override string ToString() =>
            $"{FileName} ({Size} байт, змінено: {LastModified:yyyy-MM-dd HH:mm}, Hash: {Hash})";
    }

    public class FileMetadataCache
    {
        // Використовуємо OrdinalIgnoreCase, бо в Windows імена файлів нечутливі до регістру
        private readonly Dictionary<string, FileMetadata> _cache = new(StringComparer.OrdinalIgnoreCase);

        public void Add(string filePath, FileMetadata metadata)
        {
            _cache[filePath] = metadata;
        }

        public FileMetadata? Get(string filePath)
        {
            return _cache.TryGetValue(filePath, out var metadata) ? metadata : null;
        }

        public bool Remove(string filePath)
        {
            return _cache.Remove(filePath);
        }

        public bool Contains(string filePath)
        {
            return _cache.ContainsKey(filePath);
        }

        public int Count => _cache.Count;
    }
}
