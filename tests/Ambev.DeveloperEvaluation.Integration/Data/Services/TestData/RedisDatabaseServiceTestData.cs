using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Data.Services.TestData
{
    public static class RedisDatabaseServiceTestData
    {
        private static readonly Faker<Product> faker = new Faker<Product>()
            .CustomInstantiator(f => new Product
            {
                Id = f.Random.Guid(),
                Name = f.Commerce.ProductName(),
                Description = f.Commerce.ProductDescription(),
                Price = f.Random.Decimal(50, 800)
            });

        public static Product GenerateProduct()
        {
            var product = faker.Generate();
            return product;
        }
    }
}
