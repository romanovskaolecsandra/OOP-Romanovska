using System;
using System.Collections.Generic;
using lab12v2;
using Xunit;

namespace lab12v2.Tests
{
    public class InMemoryRepositoryTests
    {
        private readonly InMemoryRepository<User> _repository;

        public InMemoryRepositoryTests()
        {
            _repository = new InMemoryRepository<User>();
        }

        [Fact]
        public void Add_ShouldAddNewElement()
        {
            // Arrange
            var user = new User { Id = 1, Username = "test", Email = "test@test.com", IsActive = true };

            // Act
            _repository.Add(user);
            var result = _repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Username);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            // Act
            var result = _repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllElements()
        {
            // Arrange
            _repository.Add(new User { Id = 1, Username = "u1" });
            _repository.Add(new User { Id = 2, Username = "u2" });

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Equal(2, ((List<User>)result).Count);
        }

        [Fact]
        public void Update_ShouldModifyExistingElement()
        {
            // Arrange
            var user = new User { Id = 1, Username = "old_name" };
            _repository.Add(user);

            // Act
            var updatedUser = new User { Id = 1, Username = "new_name" };
            _repository.Update(updatedUser);
            var result = _repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new_name", result.Username);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenEntityDoesNotExist()
        {
            // Arrange
            var user = new User { Id = 999, Username = "ghost" };

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(user));
        }

        [Fact]
        public void Delete_ShouldRemoveElement()
        {
            // Arrange
            var user = new User { Id = 1, Username = "to_delete" };
            _repository.Add(user);

            // Act
            _repository.Delete(1);
            var result = _repository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenEntityDoesNotExist()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(999));
        }
    }
}
