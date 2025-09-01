using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
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
            int itemQuantity = SaleItemTestData.GenerateValidItemQuantity(21,50);
            decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
            var saleItems = new List<SaleItem> {
                new SaleItem { ProductId = productId, ProductItemQuantity = itemQuantity, UnitProductItemPrice = new Money(price) }
            };

            Assert.Equal(false, spec.IsSatisfiedBy(saleItems));
        }

        /// <summary>
        /// Should return true when the total quantity of product items exceeds the maximum allowed.
        /// </summary>
        [Fact]
        public void IsSatisfiedBy_ReturnsTrue_WhenQuantityExceedsMaximum()
        {
            // Arrange
            var saleItems = new List<SaleItem>
            {
                new SaleItem { ProductId = SaleItemTestData.GenerateValidProductId(), ProductItemQuantity = 51 }
            };
            var specification = new MaxQuantityProductItemsSpecification();

            // Act
            var result = specification.IsSatisfiedBy(saleItems);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Should return false when the total quantity of product items is within the allowed limit.
        /// </summary>
        [Fact]
        public void IsSatisfiedBy_ReturnsFalse_WhenQuantityIsWithinLimit()
        {
            // Arrange
            var saleItems = new List<SaleItem>
            {
                new SaleItem { ProductId = SaleItemTestData.GenerateValidProductId(), ProductItemQuantity = 10 },
                new SaleItem { ProductId = SaleItemTestData.GenerateValidProductId(), ProductItemQuantity = 20 }
            };
            var specification = new MaxQuantityProductItemsSpecification();

            // Act
            var result = specification.IsSatisfiedBy(saleItems);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Should return false when the sale items list is empty.
        /// </summary>
        [Fact]
        public void IsSatisfiedBy_ReturnsFalse_WhenSaleItemsIsEmpty()
        {
            // Arrange
            var saleItems = new List<SaleItem>();
            var specification = new MaxQuantityProductItemsSpecification();

            // Act
            var result = specification.IsSatisfiedBy(saleItems);

            // Assert
            Assert.True(result);
        }
    }
}
