using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<Cart?> UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<Cart>> GetPagedAsync(QueryOptions options, CancellationToken cancellationToken = default);
        Task UpdateBranchInCartAsync(Guid branchId, string newBranchName, CancellationToken cancellationToken = default);
    }
}
