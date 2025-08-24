using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Data.Repositories
{
    public class SaleRepositoryTests
    {
    
        private DefaultContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new DefaultContext(options);
        }

        [Fact(DisplayName = "Should create a sale successfully")]
        public async Task CreateAsync_ShouldAddSale()
        {
            using var context = CreateContext();
            var repository = new SaleRepository(context);
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                TotalSalePrice = 100,
                BranchSaleId = Guid.NewGuid()
            };
            sale.AddSaleItems(new List<SaleItem>());

            var result = await repository.CreateAsync(sale);

            Assert.NotNull(await context.Sales.FindAsync(sale.Id));
            Assert.Equal(sale.SaleNumber, result.SaleNumber);
        }

        [Fact(DisplayName = "Should return sale by id when it exists")]
        public async Task GetByIdAsync_ShouldReturnSale_WhenExists()
        {
            using var context = CreateContext();
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                TotalSalePrice = 200,
                BranchSaleId = Guid.NewGuid()
            };
            context.Sales.Add(sale);
            context.SaveChanges();
            var repository = new SaleRepository(context);

            var result = await repository.GetByIdAsync(sale.Id);

            Assert.NotNull(result);
            Assert.Equal(sale.Id, result.Id);
        }

        [Fact(DisplayName = "Should return null when sale does not exist")]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            using var context = CreateContext();
            var repository = new SaleRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should delete sale when it exists")]
        public async Task DeleteAsync_ShouldRemoveSale_WhenExists()
        {
            using var context = CreateContext();
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                TotalSalePrice = 300,
                BranchSaleId = Guid.NewGuid()
            };
            context.Sales.Add(sale);
            context.SaveChanges();
            var repository = new SaleRepository(context);

            var deleted = await repository.DeleteAsync(sale.Id);

            Assert.True(deleted);
            Assert.Null(await context.Sales.FindAsync(sale.Id));
        }

        [Fact(DisplayName = "Should return false when deleting non-existent sale")]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            using var context = CreateContext();
            var repository = new SaleRepository(context);

            var deleted = await repository.DeleteAsync(Guid.NewGuid());

            Assert.False(deleted);
        }

        [Fact(DisplayName = "Should throw NotImplementedException on update")]
        public async Task UpdateAsync_ShouldThrowNotImplementedException()
        {
            using var context = CreateContext();
            var repository = new SaleRepository(context);
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                TotalSalePrice = 400,
                BranchSaleId = Guid.NewGuid()
            };
            sale.UpdateSale();

            await Assert.ThrowsAsync<NotImplementedException>(() => repository.UpdateAsync(sale));
        }
    }
}
