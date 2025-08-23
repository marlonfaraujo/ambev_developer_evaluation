using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTest
    {
        [Fact(DisplayName = "With fake data sale item generation, sale should give discount if quantity > 4 and <= 20")]
        public void Given_SaleItemsWithCustomData_When_IdenticalProducts_Then_ShouldBeTheSameTotalSaleItemPrice()
        {
            int productId = SaleItemTestData.GenerateValidProductId();
            int itemQuantity = SaleItemTestData.GenerateValidItemQuantity();
            decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
            var saleItem = SaleItemTestData.GenerateValidSaleItem(productId, itemQuantity, price);
            decimal discountAmount = (itemQuantity > 10 && itemQuantity <= 20) ? 0.2m : (itemQuantity > 4 ) ? 0.1m : 0;
            decimal expectedTotal = (price * itemQuantity) - ((price * itemQuantity) * discountAmount);
            decimal totalSaleItemPrice = saleItem.CalculateTotalSaleItemPrice();
            totalSaleItemPrice.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Must ungroup the list with sales items because the product identifiers are duplicated and there should be no exception if the value is null")]
        public void Given_SaleItems_When_ProductIdDuplicated_Then_GroupAndSumQuantityOfProducts()
        {
            var saleItemsWithoutGrouping = new List<SaleItem>
            {
                new SaleItem { ProductId = 1, ProductItemQuantity = 3, UnitProductItemPrice = 30 },
                new SaleItem { ProductId = 1, ProductItemQuantity = 5, UnitProductItemPrice = 100 },
                new SaleItem { ProductId = 1, ProductItemQuantity = 15, UnitProductItemPrice = 200 }
            };
            var saleItemsGrouped = SaleItem.GetSaleItemsGroupedByProductId(saleItemsWithoutGrouping);
            var totalQuantity = saleItemsWithoutGrouping.Sum(x => x.ProductItemQuantity);
            totalQuantity.Should().Be(saleItemsGrouped.Sum(x => x.ProductItemQuantity));
            Assert.NotNull(SaleItem.GetSaleItemsGroupedByProductId(null));
        }
    }
}
