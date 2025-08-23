using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleItemTestData
    {
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .RuleFor(u => u.ProductId, f => f.Random.Int())
            .RuleFor(u => u.SaleItemStatus, f => f.Random.String());

        public static SaleItem GenerateValidSaleItem(int productId, int itemQuantity, decimal unitItemPrice)
        {
            SaleItemFaker.RuleFor(u => u.ProductId, f => productId);
            SaleItemFaker.RuleFor(u => u.ProductItemQuantity, f => itemQuantity);
            SaleItemFaker.RuleFor(u => u.UnitProductItemPrice, f => unitItemPrice);

            return SaleItemFaker.Generate();
        }

        public static int GenerateValidProductId()
        {
            return new Faker().Random.Int(1, 100);
        }

        public static int GenerateValidItemQuantity()
        {
            return new Faker().Random.Int(1, 30);
        }

        public static decimal GenerateValidUnitItemPrice()
        {
            return new Faker().Random.Decimal(50,100);
        }
    }
}
