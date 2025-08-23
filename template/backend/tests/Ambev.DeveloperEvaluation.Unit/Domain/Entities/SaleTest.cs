using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
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

    [Fact(DisplayName = "With personalized data, the sale should give a discount because there are identical products, with 16 and 4 quantities")]
    public void Given_SaleItemsWithCustomData_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleItems = new List<SaleItem>
        {
            SaleItemTestData.GenerateValidSaleItem(SaleItemTestData.GenerateValidProductId(), 16, 90.29m),
            SaleItemTestData.GenerateValidSaleItem(SaleItemTestData.GenerateValidProductId(), 4, 122.73m)
        };
        decimal price1 = (90.29m * 16) - ((90.29m * 16) * 0.2m);
        decimal price2 = (122.73m * 4) - ((122.73m * 4) * 0.1m);
        decimal expectedTotal = price1 + price2;
        decimal totalSalePrice = sale.CalculateTotalSalePriceWithItems();
        totalSalePrice.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "When trying to calculate the total value of the items, it should not allow it to continue if there are more than 20 products")]
    public void Given_CalculateTotalSalePriceWithItems_When_HaveMoreThanMaxQuantityItems_Then_ThereIsAnException()
    {
        int productId = SaleItemTestData.GenerateValidProductId();
        decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleItems = new List<SaleItem>
        {
            SaleItemTestData.GenerateValidSaleItem(productId, 20, price),
            SaleItemTestData.GenerateValidSaleItem(productId, 20, price)
        };
        var ex = Assert.Throws<MaxQuantityProductItemsException>(() => sale.CalculateTotalSalePriceWithItems());
        Assert.Equal("The maximum quantity of product items is 20.", ex.Message);
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
        var sale = new Sale()
        {
            SaleItems = saleItemsWithoutGrouping
        };
        var saleItemsGrouped = sale.GetSaleItemsGroupedByProductId();
        var totalQuantity = saleItemsWithoutGrouping.Sum(x => x.ProductItemQuantity);
        totalQuantity.Should().Be(saleItemsGrouped.Sum(x => x.ProductItemQuantity));
        Assert.NotNull(sale.GetSaleItemsGroupedByProductId());
    }
}
