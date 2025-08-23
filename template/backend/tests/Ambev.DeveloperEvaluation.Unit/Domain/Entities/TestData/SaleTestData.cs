using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .RuleFor(u => u.UserId, f => f.Random.Guid())
            .RuleFor(u => u.BranchSaleId, f => f.Random.Int())
            .RuleFor(u => u.SaleStatus, f => f.Random.String());

        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

    }
}
