namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface IMongoDbServiceFactory
    {
        IMongoDbService<T> Create<T>(string collectionName);
    }
}
