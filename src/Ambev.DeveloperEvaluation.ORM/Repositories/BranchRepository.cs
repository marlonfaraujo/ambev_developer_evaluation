using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DefaultContext _context;
        public BranchRepository(DefaultContext context)
        {
            _context = context;
        }
        public async Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default)
        {
            await _context.Branchs.AddAsync(branch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return branch;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var branch = await GetByIdAsync(id, cancellationToken);
            if (branch == null)
                return false;

            _context.Branchs.Remove(branch);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Branchs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Branch?> UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
        {
            var local = _context.Set<Branch>()
                .Local
                .FirstOrDefault(entry => entry.Id == branch.Id);

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Entry(branch).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return branch;
        }

        public async Task<PagedResult<Branch>> GetPagedAsync(QueryOptions options, CancellationToken cancellationToken = default)
        {
            var query = _context.Branchs.AsQueryable();
            if (options.Filters != null && options.Filters.Any())
            {
                foreach (var kv in options.Filters)
                {
                    var property = typeof(Branch).GetProperty(kv.Key);
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
                            query = query.Where(x => EF.Property<string>(x, kv.Key) == value.ToString());
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(options.SortBy))
            {
                var property = typeof(Branch).GetProperty(options.SortBy);
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

            return new PagedResult<Branch>
            {
                Items = items,
                TotalCount = totalCount,
                Page = options.Page,
                PageSize = options.PageSize
            };
        }
    }
}
