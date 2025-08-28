using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSql
{
    public class MongoDbService<T>
    {
        private readonly MongoDbContext _mongoDb;
        private readonly IMongoCollection<T> _collection;

        public MongoDbService(MongoDbContext mongoDb, string collectionName)
        {
            _mongoDb = mongoDb;
            _collection = _mongoDb.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }
    
    }
}
