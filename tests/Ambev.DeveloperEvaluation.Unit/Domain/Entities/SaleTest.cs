using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit;

public class SaleTest
{
    [Fact(DisplayName = "Sale should give discount if it has identical products")]
    public void Given_SaleItems_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        var sale = new Sale();
        sale.AddSaleItems(new List<SaleItem>
            {
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 3, UnitProductItemPrice = 30 },
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 5, UnitProductItemPrice = 100 },
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 15, UnitProductItemPrice = 200 }
            }
        );
        var simulateSaleService = new SimulateSaleService(sale, Enumerable.Empty<Product>());
        var totalPrice = simulateSaleService.TotalSalePrice();
        Assert.Equal(totalPrice, 2940.00m);

        var sale2 = new Sale();
        sale2.AddSaleItems(
            new List<SaleItem>
            {
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 3, UnitProductItemPrice = 30.50m },
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 5, UnitProductItemPrice = 100.98m },
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 15, UnitProductItemPrice = 200.59m }
            }
        );
        var simulateSaleService2 = new SimulateSaleService(sale2, Enumerable.Empty<Product>());
        totalPrice = simulateSaleService2.TotalSalePrice();
        Assert.Equal(totalPrice, 2952.99m);
    }

    [Fact(DisplayName = "With fake data generation, sale should give discount if it has identical products")]
    public void Given_SaleItemsWithGenerateFaker_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        Guid productId = SaleItemTestData.GenerateValidProductId();
        int itemQuantity = SaleItemTestData.GenerateValidItemQuantity(1);
        decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
        var sale = SaleTestData.GenerateValidSale();
        var saleItem = SaleItemTestData.GenerateValidSaleItem(productId, itemQuantity, price);
        sale.AddSaleItems(new List<SaleItem> { saleItem });
        decimal expectedSaleItemPrice = saleItem.CalculateTotalSaleItemPrice();

        var simulateSaleService = new SimulateSaleService(sale, Enumerable.Empty<Product>());
        decimal totalSalePrice = simulateSaleService.TotalSalePrice();
        Assert.Equal(totalSalePrice, expectedSaleItemPrice);
    }

    [Fact(DisplayName = "With personalized data, the sale should give a discount because there are identical products, with 16 and 4 quantities")]
    public void Given_SaleItemsWithCustomData_When_IdenticalProducts_Then_ShouldHaveTotalPriceWithDiscount()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.AddSaleItems(new List<SaleItem>
            {
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 16, UnitProductItemPrice = 90.29m },
                new SaleItem { ProductId = Guid.NewGuid(), ProductItemQuantity = 4, UnitProductItemPrice = 122.73m }
            }
        );
        decimal price1 = (90.29m * 16) - ((90.29m * 16) * 0.2m);
        decimal price2 = (122.73m * 4) - ((122.73m * 4) * 0.1m);
        decimal expectedTotal = price1 + price2;
        var simulateSaleService = new SimulateSaleService(sale, Enumerable.Empty<Product>());
        decimal totalSalePrice = simulateSaleService.TotalSalePrice();
        Assert.Equal(expectedTotal, totalSalePrice);
    }

    [Fact(DisplayName = "When trying to calculate the total value of the items, it should not allow it to continue if there are more than 20 products")]
    public void Given_CalculateTotalSalePriceWithItems_When_HaveMoreThanMaxQuantityItems_Then_ThereIsAnException()
    {
        decimal price = SaleItemTestData.GenerateValidUnitItemPrice();
        var sale = SaleTestData.GenerateValidSale();
        sale.AddSaleItems(new List<SaleItem>
            {
                SaleItemTestData.GenerateValidSaleItem(SaleItemTestData.GenerateValidProductId(), SaleItemTestData.GenerateValidItemQuantity(21,30), price),
                SaleItemTestData.GenerateValidSaleItem(SaleItemTestData.GenerateValidProductId(), SaleItemTestData.GenerateValidItemQuantity(21,30), price)
            }
        );
        var simulateSaleService = new SimulateSaleService(sale, Enumerable.Empty<Product>());
        var ex = Assert.Throws<MaxQuantityProductItemsException>(() => simulateSaleService.ValidateMaxQuantityProductItems(sale.SaleItems));
        Assert.Equal("The maximum quantity of product items is 20.", ex.Message);
    }


    [Fact(DisplayName = "Must ungroup the list with sales items because the product identifiers are duplicated and there should be no exception if the value is null")]
    public void Given_SaleItems_When_ProductIdDuplicated_Then_GroupAndSumQuantityOfProducts()
    {
        var duplicatedProductId = Guid.NewGuid();
        var saleItemsWithoutGrouping = new List<SaleItem>
        {
            new SaleItem { ProductId = duplicatedProductId, ProductItemQuantity = 3, UnitProductItemPrice = 30 },
            new SaleItem { ProductId = duplicatedProductId, ProductItemQuantity = 5, UnitProductItemPrice = 100 },
            new SaleItem { ProductId = duplicatedProductId, ProductItemQuantity = 15, UnitProductItemPrice = 200 }
        };
        var totalQuantity = saleItemsWithoutGrouping.Sum(x => x.ProductItemQuantity);
        var sale = new Sale();
        sale.AddSaleItems(saleItemsWithoutGrouping);
        Assert.NotNull(sale.SaleItems);
        Assert.Equal(totalQuantity, sale.SaleItems.Sum(x => x.ProductItemQuantity));
    }
}
