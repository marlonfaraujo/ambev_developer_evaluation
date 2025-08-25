using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Application.TestData
{
    public static class SaleHandlerTestData
    {
        public static Sale GetSale()
        {
            using var context = new DatabaseInMemory().Context;
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

            var result = repository.CreateAsync(sale).Result;
            return result;
        }
    }
}
