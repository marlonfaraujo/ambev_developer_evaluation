using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Common;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSql
{
    public class MongoDbService<T>: IMongoDbService<T>
    {
        private readonly MongoDbContext _mongoDb;
        private IMongoCollection<T> _collection;

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

        public async Task<PagedResult<T>> QueryAsync(QueryOptions options)
        {
            var filterBuilder = Builders<T>.Filter;
            var filter = filterBuilder.Empty;

            if (options.Filters != null && options.Filters.Any())
            {
                var filters = new List<FilterDefinition<T>>();
                foreach (var kv in options.Filters)
                {
                    if (typeof(T).GetProperty(kv.Key)?.PropertyType == typeof(Guid) || kv.Value?.GetType() == typeof(Guid))
                    {
                        var guidValue = Guid.Parse(kv.Value.ToString()!);
                        filters.Add(filterBuilder.Eq(kv.Key, guidValue));
                    }
                    else
                    {
                        filters.Add(filterBuilder.Eq(kv.Key, kv.Value));
                    }
                }
                filter = filterBuilder.And(filters);
            }

            var query = _collection.Find(filter);
            if (!string.IsNullOrEmpty(options.SortBy))
            {
                query = options.SortDescending
                    ? query.Sort(Builders<T>.Sort.Descending(options.SortBy))
                    : query.Sort(Builders<T>.Sort.Ascending(options.SortBy));
            }

            var totalCount = await query.CountDocumentsAsync();
            var items = await query
                .Skip((options.Page - 1) * options.PageSize)
                .Limit(options.PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = options.Page,
                PageSize = options.PageSize
            };
        }
    }
}
