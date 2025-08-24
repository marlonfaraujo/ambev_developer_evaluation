namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface IQueryDatabaseService
    {
        Task<IEnumerable<TEntity>> Select<TEntity>(string query, params object[] parameters) where TEntity : class;
        Task<bool> ProductsInItem(string productId);
        Task<bool> BranchsInItem(string branchId);
    }
}
