using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<Cart?> UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<Cart>> GetPagedAsync(int page, int pageSize, Dictionary<string, object>? filters = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default);
    }
}
