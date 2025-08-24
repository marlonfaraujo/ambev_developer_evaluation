using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Data.Services.TestData;
using Ambev.DeveloperEvaluation.ORM.Services;
using StackExchange.Redis;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Data.Services
{
    public class RedisDatabaseServiceTests
    {
        private readonly RedisDatabaseService _redisDatabaseService;

        public RedisDatabaseServiceTests()
        {
            _redisDatabaseService = new RedisDatabaseService(ConnectionMultiplexer.Connect("localhost:6379,password=ev@luAt10n"));
        }

        /// <summary>
        /// Should store and retrieve a value successfully.
        /// </summary>
        [Fact(DisplayName = "SetAsync and GetAsync should store and retrieve a value")]
        public async Task SetAndGetAsync_ShouldStoreAndRetrieveValue()
        {
            // Arrange
            var key = Guid.NewGuid();
            var value = RedisDatabaseServiceTestData.GenerateProduct();

            await _redisDatabaseService.SetAsync(key.ToString(), value, TimeSpan.FromMinutes(60));
            var result = await _redisDatabaseService.GetAsync<Product>(key.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(value.Id, result!.Id);
            Assert.Equal(value.Name, result.Name);
        }

        /// <summary>
        /// Should return null when getting a non-existent key.
        /// </summary>
        [Fact(DisplayName = "GetAsync should return null for non-existent key")]
        public async Task GetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = Guid.NewGuid();
            var value = RedisDatabaseServiceTestData.GenerateProduct();

            // Act
            var result = await _redisDatabaseService.GetAsync<Product>(key.ToString());

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Should remove a key successfully.
        /// </summary>
        [Fact(DisplayName = "RemoverAsync should remove a key")]
        public async Task RemoverAsync_ShouldRemoveKey()
        {
            // Arrange
            var key = Guid.NewGuid();
            var value = RedisDatabaseServiceTestData.GenerateProduct();
            var json = JsonSerializer.Serialize(value);
            await _redisDatabaseService.SetAsync(key.ToString(), json);

            // Act
            var removed = await _redisDatabaseService.RemoverAsync(key.ToString());
            var result = await _redisDatabaseService.GetAsync<Product>(key.ToString());

            // Assert
            Assert.True(removed);
            Assert.Null(result);
        }

    }
}
