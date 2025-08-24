namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface IQueryDatabaseService
    {
        Task<IEnumerable<TEntity>> Select<TEntity>(string query, params object[] parameters) where TEntity : class;
        Task<bool> ProductsInItem(Guid productId);
        Task<bool> BranchsInItem(Guid branchId);
        object[] GetSqlParameters<T>(T parameters);
    }
}
