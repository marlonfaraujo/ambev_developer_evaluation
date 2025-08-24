using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Data.Repositories
{
    public class ProductRepositoryTests
    {
        private DefaultContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new DefaultContext(options);
        }

        [Fact(DisplayName = "Should create a product successfully")]
        public async Task CreateAsync_ShouldAddProduct()
        {
            using var context = CreateContext();
            var repository = new ProductRepository(context);
            var product = new Product(Guid.NewGuid(), "Product 1", "Description 1", 10.5m);

            var result = await repository.CreateAsync(product);

            Assert.NotNull(await context.Products.FindAsync(product.Id));
            Assert.Equal(product.Name, result.Name);
        }

        [Fact(DisplayName = "Should return product by id when it exists")]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            using var context = CreateContext();
            var product = new Product(Guid.NewGuid(), "Product 2", "Description 2", 20.0m);
            context.Products.Add(product);
            context.SaveChanges();
            var repository = new ProductRepository(context);

            var result = await repository.GetByIdAsync(product.Id);

            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact(DisplayName = "Should return null when product does not exist")]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            using var context = CreateContext();
            var repository = new ProductRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should delete product when it exists")]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenExists()
        {
            using var context = CreateContext();
            var product = new Product(Guid.NewGuid(), "Product 3", "Description 3", 30.0m);
            context.Products.Add(product);
            context.SaveChanges();
            var repository = new ProductRepository(context);

            var deleted = await repository.DeleteAsync(product.Id);

            Assert.True(deleted);
            Assert.Null(await context.Products.FindAsync(product.Id));
        }

        [Fact(DisplayName = "Should return false when deleting non-existent product")]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            using var context = CreateContext();
            var repository = new ProductRepository(context);

            var deleted = await repository.DeleteAsync(Guid.NewGuid());

            Assert.False(deleted);
        }

        [Fact(DisplayName = "Should return products by ids")]
        public async Task ListByIdsAsync_ShouldReturnProducts_WhenIdsExist()
        {
            using var context = CreateContext();
            var product1 = new Product(Guid.NewGuid(), "Product 4", "Description 4", 40.0m);
            var product2 = new Product(Guid.NewGuid(), "Product 5", "Description 5", 50.0m);
            context.Products.AddRange(product1, product2);
            context.SaveChanges();
            var repository = new ProductRepository(context);

            var result = await repository.ListByIdsAsync(new[] { product1.Id, product2.Id });

            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Contains(resultList, p => p.Id == product1.Id);
            Assert.Contains(resultList, p => p.Id == product2.Id);
        }

        [Fact(DisplayName = "Should return empty list when no ids match")]
        public async Task ListByIdsAsync_ShouldReturnEmpty_WhenNoIdsMatch()
        {
            using var context = CreateContext();
            var repository = new ProductRepository(context);

            var result = await repository.ListByIdsAsync(new[] { Guid.NewGuid(), Guid.NewGuid() });

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should throw NotImplementedException on update")]
        public async Task UpdateAsync_ShouldThrowNotImplementedException()
        {
            using var context = CreateContext();
            var repository = new ProductRepository(context);
            var product = new Product(Guid.NewGuid(), "Product 6", "Description 6", 60.0m);

            await Assert.ThrowsAsync<NotImplementedException>(() => repository.UpdateAsync(product));
        }
    }
}
