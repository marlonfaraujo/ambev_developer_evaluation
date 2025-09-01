using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Common;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagedResult<Cart>> GetPagedAsync(QueryOptions options, CancellationToken cancellationToken = default)
        {
            var query = _context.Carts.Include(x => x.CartItems).AsQueryable();
            if (options.Filters != null && options.Filters.Any())
            {
                foreach (var kv in options.Filters)
                {
                    if (kv.Key.StartsWith("CartItems."))
                    {
                        var propertyName = kv.Key.Replace("CartItems.", "");
                        var property = typeof(CartItem).GetProperty(propertyName);

                        if (property != null)
                        {
                            var value = kv.Value;
                            if (property.PropertyType == typeof(Guid) && value != null)
                            {
                                var guidValue = Guid.Parse(value.ToString()!);
                                query = query.Where(c => c.CartItems.Any(ci => EF.Property<Guid>(ci, propertyName) == guidValue));
                            }
                            else
                            {
                                query = query.Where(c => c.CartItems.Any(ci => EF.Property<string>(ci, propertyName) == value!.ToString()));
                            }
                        }
                    } 
                    else
                    {
                        var property = typeof(Cart).GetProperty(kv.Key);
                        if (property != null)
                        {
                            var value = kv.Value;
                            if (property.PropertyType == typeof(Guid) && value != null)
                            {
                                var guidValue = Guid.Parse(value.ToString()!);
                                query = query.Where(x => EF.Property<Guid>(x, kv.Key) == guidValue);
                            }
                            else
                            {
                                query = query.Where(x => EF.Property<string>(x, kv.Key) == value!.ToString());
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(options.SortBy))
            {
                var property = typeof(Cart).GetProperty(options.SortBy);
                if (property != null)
                {
                    if (options.SortDescending)
                        query = query.OrderByDescending(x => EF.Property<object>(x, options.SortBy));
                    else
                        query = query.OrderBy(x => EF.Property<object>(x, options.SortBy));
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((options.Page - 1) * options.PageSize).Take(options.PageSize).ToListAsync(cancellationToken);

            return new PagedResult<Cart>
            {
                Items = items,
                TotalCount = totalCount,
                Page = options.Page,
                PageSize = options.PageSize
            };
        }

        public async Task UpdateBranchInCartAsync(Guid branchId, string newBranchName, CancellationToken cancellationToken = default)
        {
            await _context.Carts
                .Where(x => x.BranchSaleId == branchId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.BranchName, _ => newBranchName));
        }
    }
}
