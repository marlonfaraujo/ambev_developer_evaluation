using Ambev.DeveloperEvaluation.Domain.Entities;
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
                new SaleItem { ProductId = 1, ItemQuantity = 3, UnitItemPrice = 30 },
                new SaleItem { ProductId = 2, ItemQuantity = 5, UnitItemPrice = 100 },
                new SaleItem { ProductId = 3, ItemQuantity = 15, UnitItemPrice = 200 }
            }
        };

        var totalPrice = sale.CalculateTotalPrice();
        Assert.Equal(totalPrice, 2940.00m);

        var sale2 = new Sale
        {
            SaleItems = new List<SaleItem>
            {
                new SaleItem { ProductId = 1, ItemQuantity = 3, UnitItemPrice = 30.50m },
                new SaleItem { ProductId = 2, ItemQuantity = 5, UnitItemPrice = 100.98m },
                new SaleItem { ProductId = 3, ItemQuantity = 15, UnitItemPrice = 200.59m }
            }
        };

        totalPrice = sale2.CalculateTotalPrice();
        Assert.Equal(totalPrice, 2952.99m);
    }
}
