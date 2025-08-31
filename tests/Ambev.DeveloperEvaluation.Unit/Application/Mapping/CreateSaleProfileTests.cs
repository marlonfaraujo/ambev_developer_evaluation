using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using FluentAssertions;
using Xunit;

public class CreateSaleProfileTests
{
    private readonly IMapper _mapper;

    public CreateSaleProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreateSaleProfile>(); 
        });

        config.AssertConfigurationIsValid(); 

        _mapper = config.CreateMapper();
    }

    [Fact(Skip = "Ignore mappging test")]
    public void Should_Map_CartItem_To_SaleItem()
    {
        // Arrange
        var cartItem = new CartItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Produto Teste",
            ProductItemQuantity = 2,
            UnitProductItemPrice = new Money(10)
        };

        // Act
        var saleItem = _mapper.Map<SaleItem>(cartItem);

        // Assert
        saleItem.ProductId.Should().Be(cartItem.ProductId);
        saleItem.ProductItemQuantity.Should().Be(2);
        saleItem.UnitProductItemPrice.Should().Be(10m);
        saleItem.SaleItemStatus.Should().Be(SaleStatusEnum.Created.ToString()); 
    }


    [Fact(Skip = "Ignore")]
    public void Should_Map_Cart_To_CreateSaleCommand()
    {
        // Arrange
        var cart = new Cart(Guid.NewGuid(), new Money(10), Guid.NewGuid(), "Branch");
        cart.CartItems.Add(
            new CartItem(
                Guid.NewGuid(),
                "Product",
                10,
                new Money(10),
                new Money(0),
                new Money((10 * 10)),
                new Money((10 * 10))
            )
        );

        // Act
        var command = _mapper.Map<CreateSaleCommand>(cart);

        // Assert
        command.BranchSaleId.Should().Be(cart.BranchSaleId);
        command.UserId.Should().Be(cart.UserId);
        command.TotalSalePrice.Should().Be(10m);
        command.SaleItems.Select(x => x.ProductId).First().Should().Be(cart.CartItems.Select(x => x.ProductId).First());
    }
}
