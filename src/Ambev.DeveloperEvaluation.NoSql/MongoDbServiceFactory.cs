using Ambev.DeveloperEvaluation.Application.Services;

namespace Ambev.DeveloperEvaluation.NoSql
{
    public class MongoDbServiceFactory : IMongoDbServiceFactory
    {
        private readonly MongoDbContext _mongoDb;

        public MongoDbServiceFactory(MongoDbContext mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public IMongoDbService<T> Create<T>(string collectionName)
        {
            return new MongoDbService<T>(_mongoDb, collectionName);
        }
    }
}
