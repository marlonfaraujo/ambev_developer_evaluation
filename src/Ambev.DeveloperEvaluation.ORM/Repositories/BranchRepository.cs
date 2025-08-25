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
    }
}
