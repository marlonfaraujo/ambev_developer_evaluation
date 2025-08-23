using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTest
    {
        [Fact(DisplayName = "With fake data sale item generation, sale should give discount if quantity >= 4 and <= 20")]
        public void Given_SaleItemsWithCustomData_When_IdenticalProducts_Then_ShouldBeTheSameTotalSaleItemPrice()
        {
            int productId = SaleItemTestData.GenerateValidProductId();
            int itemQuantity = SaleItemTestData.GenerateValidItemQuantity();
            decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
            var saleItem = SaleItemTestData.GenerateValidSaleItem(productId, itemQuantity, price);
            decimal discountAmount = (itemQuantity > 10 && itemQuantity <= 20) ? 0.2m : (itemQuantity >= 4 ) ? 0.1m : 0;
            decimal expectedTotal = (price * itemQuantity) - ((price * itemQuantity) * discountAmount);
            decimal totalSaleItemPrice = saleItem.CalculateTotalSaleItemPrice();
            totalSaleItemPrice.Should().Be(expectedTotal);
        }
    }
}
