using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Services
{
    public class RedisDatabaseService
    {
        private readonly IDatabase _database;

        public RedisDatabaseService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiry);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task<bool> RemoverAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }
    }
}
