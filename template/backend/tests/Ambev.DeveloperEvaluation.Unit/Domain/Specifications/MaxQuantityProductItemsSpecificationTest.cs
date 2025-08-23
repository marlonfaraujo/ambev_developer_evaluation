using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class MaxQuantityProductItemsSpecificationTest
    {
        [Fact(DisplayName = "MaxQuantityProductItemsSpecificationTest")]
        public void Given_QuantityOfItems_When_ItIsGreaterThan20_Then_ErrorMustOccur()
        {
            var spec = new MaxQuantityProductItemsSpecification();

            Guid productId = SaleItemTestData.GenerateValidProductId();
            int itemQuantity = SaleItemTestData.GenerateValidItemQuantity(20,50);
            decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
            var saleItems = new List<SaleItem> {
                SaleItemTestData.GenerateValidSaleItem(productId, itemQuantity, price)
            };

            Assert.Equal(spec.IsSatisfiedBy(saleItems), false);
        }
    }
}
