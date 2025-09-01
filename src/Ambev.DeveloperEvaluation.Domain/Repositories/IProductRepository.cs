using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
        Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>?> ListByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
        Task<PagedResult<Product>> GetPagedAsync(QueryOptions options, CancellationToken cancellationToken = default);
    }
}
