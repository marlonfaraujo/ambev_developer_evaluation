using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Application.TestData
{
    public static class ProductHandlerTestData
    {

        public static Product GetProduct()
        {
            using var context = new DatabaseInMemory().Context;
            var repository = new ProductRepository(context);
            var product = new Product(Guid.NewGuid(), "Product 1", "Description 1", 10.5m);

            var result = repository.CreateAsync(product).Result;
            return result;
        }
    }
}
