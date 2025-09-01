using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSql
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string dbName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("MongoDB connection string is required.", nameof(connectionString));

            Configuration();

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        private void Configuration()
        {
            try
            {
                BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
            }
            catch (BsonSerializationException exception)
            {
                Console.WriteLine($"BsonSerializationException Exception: {exception.Message}");
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(SaleModel)))
            {
                BsonClassMap.RegisterClassMap<SaleModel>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetDiscriminatorIsRequired(false);
                });

                BsonClassMap.RegisterClassMap<SaleItemModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminatorIsRequired(false);
                });
            }
        }

    }
}
