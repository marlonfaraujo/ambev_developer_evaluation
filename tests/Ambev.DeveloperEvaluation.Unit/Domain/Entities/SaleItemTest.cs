using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTest
    {
        [Fact(DisplayName = "With fake data sale item generation, sale should give discount if quantity >= 4 and <= 20")]
        public void Given_SaleItemsWithCustomData_When_IdenticalProducts_Then_ShouldBeTheSameTotalSaleItemPrice()
        {
            Guid productId = SaleItemTestData.GenerateValidProductId();
            int itemQuantity = SaleItemTestData.GenerateValidItemQuantity(1);
            var price = new Money(SaleItemTestData.GenerateValidUnitItemPrice());
            var saleItem = new SaleItem { ProductId = productId, ProductItemQuantity = itemQuantity, UnitProductItemPrice = price };
            decimal discountAmount = (itemQuantity >= 10 && itemQuantity <= 20) ? 0.2m : (itemQuantity >= 4 && itemQuantity <= 9) ? 0.1m : 0;
            var totalWithoutDiscount = new Money(price.Value * itemQuantity);
            var discountCalculate = new Money(totalWithoutDiscount.Value * discountAmount);
            var expectedTotal = new Money(totalWithoutDiscount.Value - discountCalculate.Value);

            decimal totalSaleItemPrice = saleItem.CalculateTotalSaleItemPrice();
            Assert.Equal(expectedTotal.Value, totalSaleItemPrice);
        }
    }
}
