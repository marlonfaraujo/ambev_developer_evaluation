using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(x => x.SaleItems)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Sale?> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            var saleExists = await _context.Sales
                .Include(x => x.SaleItems)
                .FirstOrDefaultAsync(x => x.Id == sale.Id);

            if (saleExists is null)
                throw new Exception("Sale not found");

            _context.Entry(saleExists).CurrentValues.SetValues(sale);

            saleExists.SaleItems.SyncCollection(
                sale.SaleItems,
                getKey: item => item.Id,
                updateItem: (exists, updated) =>
                {
                    _context.Entry(exists).CurrentValues.SetValues(updated);
                },
                onAdd: item => {},
                onRemove: item => _context.Remove(item)
            );

            await _context.SaveChangesAsync();

            return sale;
        }
    }
}
