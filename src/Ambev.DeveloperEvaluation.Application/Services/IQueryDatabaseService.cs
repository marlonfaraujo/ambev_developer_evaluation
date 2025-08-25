namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface IQueryDatabaseService
    {
        Task<IEnumerable<TEntity>> Select<TEntity>(string query, params object[] parameters) where TEntity : class;
        Task<bool> ProductsInSaleItems(Guid productId);
        Task<bool> BranchsInSale(Guid branchId);
        object[] GetSqlParameters<T>(T parameters);
    }
}
