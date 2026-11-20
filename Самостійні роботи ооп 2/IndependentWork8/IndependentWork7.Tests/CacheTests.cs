using System.Collections.Generic;
using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_ShouldIncreaseCount_WhenItemsAddedWithinCapacity()
        {
            var cache = new Cache<string, int>(3);

            cache.Add("one", 1);
            cache.Add("two", 2);

            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void Get_ShouldReturnCorrectValue_WhenKeyExists()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(10, "Ten");

            var result = cache.Get(10);

            Assert.Equal("Ten", result);
        }

        [Fact]
        public void Add_ShouldEvictOldestElement_WhenCapacityExceeded()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "First");
            cache.Add(2, "Second");

            cache.Add(3, "Third");

            Assert.Equal(2, cache.Count);
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
        }

        [Fact]
        public void Get_ShouldThrowKeyNotFoundException_WhenKeyDoesNotExist()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("a", "Alpha");

            Assert.Throws<KeyNotFoundException>(() => cache.Get("non_existent"));
        }

        [Fact]
        public void ContainsKey_ShouldReturnTrueForExistingAndFalseForMissingKey()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(100, "Hundred");

            Assert.True(cache.ContainsKey(100));
            Assert.False(cache.ContainsKey(200));
        }

        [Fact]
        public void Count_ShouldReturnZeroInitially_AndReflectChanges()
        {
            var cache = new Cache<int, int>(2);
            Assert.Equal(0, cache.Count);

            cache.Add(1, 10);
            Assert.Equal(1, cache.Count);

            cache.Add(2, 20);
            cache.Add(3, 30);
            Assert.Equal(2, cache.Count);
        }
    }
}
