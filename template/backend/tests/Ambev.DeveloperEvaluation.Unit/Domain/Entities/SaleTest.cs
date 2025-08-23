using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit;

public class SaleTest
{
    [Fact(DisplayName = "Sale should give discount if it has identical products")]
    public void Given_SaleItems_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        var sale = new Sale
        {
            SaleItems = new List<SaleItem>
            {
                new SaleItem { ProductId = 1, ProductItemQuantity = 3, UnitProductItemPrice = 30 },
                new SaleItem { ProductId = 2, ProductItemQuantity = 5, UnitProductItemPrice = 100 },
                new SaleItem { ProductId = 3, ProductItemQuantity = 15, UnitProductItemPrice = 200 }
            }
        };
        var totalPrice = sale.CalculateTotalSalePriceWithItems();
        Assert.Equal(totalPrice, 2940.00m);

        var sale2 = new Sale
        {
            SaleItems = new List<SaleItem>
            {
                new SaleItem { ProductId = 1, ProductItemQuantity = 3, UnitProductItemPrice = 30.50m },
                new SaleItem { ProductId = 2, ProductItemQuantity = 5, UnitProductItemPrice = 100.98m },
                new SaleItem { ProductId = 3, ProductItemQuantity = 15, UnitProductItemPrice = 200.59m }
            }
        };
        totalPrice = sale2.CalculateTotalSalePriceWithItems();
        Assert.Equal(totalPrice, 2952.99m);
    }

    [Fact(DisplayName = "With fake data generation, sale should give discount if it has identical products")]
    public void Given_SaleItemsWithGenerateFaker_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        int productId = SaleItemTestData.GenerateValidProductId();
        int itemQuantity = SaleItemTestData.GenerateValidItemQuantity();
        decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
        var sale = SaleTestData.GenerateValidSale();
        var saleItem = SaleItemTestData.GenerateValidSaleItem(productId, itemQuantity, price);
        sale.SaleItems = new List<SaleItem> { saleItem };
        decimal expectedSaleItemPrice = saleItem.CalculateTotalSaleItemPrice();
        decimal totalSalePrice = sale.CalculateTotalSalePriceWithItems();
        totalSalePrice.Should().Be(expectedSaleItemPrice);
    }


    [Fact(DisplayName = "With personalized data, the sale should give a discount because there are identical products, with 17 and 5 quantities")]
    public void Given_SaleItemsWithCustomData_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        int productId = SaleItemTestData.GenerateValidProductId();
        int itemQuantity = SaleItemTestData.GenerateValidItemQuantity();
        decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleItems = new List<SaleItem>
        {
            SaleItemTestData.GenerateValidSaleItem(productId, 17, 90.29m),
            SaleItemTestData.GenerateValidSaleItem(productId, 5, 122.73m)
        };
        decimal price1 = (90.29m * 17) - ((90.29m * 17) * 0.2m);
        decimal price2 = (122.73m * 5) - ((122.73m * 5) * 0.1m);
        decimal expectedTotal = price1 + price2;
        decimal totalSalePrice = sale.CalculateTotalSalePriceWithItems();
        totalSalePrice.Should().Be(expectedTotal);
    }
}
