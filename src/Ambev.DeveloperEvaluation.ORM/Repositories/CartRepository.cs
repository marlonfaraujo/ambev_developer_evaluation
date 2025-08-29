using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Dtos;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DefaultContext _context;
        public CartRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await _context.Carts.AddAsync(cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return cart;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cart = await GetByIdAsync(id, cancellationToken);
            if (cart == null)
                return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Cart?> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            var cartExists = await _context.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == cart.Id);

            if (cartExists is null)
                throw new Exception("Cart not found");

            _context.Entry(cartExists).CurrentValues.SetValues(cart);

            cartExists.CartItems.SyncCollection(
                cart.CartItems,
                getKey: item => item.Id,
                updateItem: (exists, updated) =>
                {
                    _context.Entry(exists).CurrentValues.SetValues(updated);
                },
                onAdd: item => {},
                onRemove: item => _context.Remove(item)
            );

            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<PagedResult<Cart>> GetPagedAsync(int page, int pageSize, Dictionary<string, object>? filters = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default)
        {
            var query = _context.Carts.Include(x => x.CartItems).AsQueryable();

            if (filters != null)
            {
                if (filters.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid userId)
                    query = query.Where(x => x.UserId == userId);
                if (filters.TryGetValue("BranchSaleId", out var branchSaleIdObj) && branchSaleIdObj is Guid branchSaleId)
                    query = query.Where(x => x.BranchSaleId == branchSaleId);
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Exemplo: só suporta sort por TotalSalePrice
                if (sortBy == "TotalSalePrice")
                    query = sortDescending ? query.OrderByDescending(x => x.TotalSalePrice.Value) : query.OrderBy(x => x.TotalSalePrice.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Cart>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
