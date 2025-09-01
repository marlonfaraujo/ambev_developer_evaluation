using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class CreateSaleHandlerTests
    {

        private readonly DefaultContext _context;

        public CreateSaleHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesSale()
        {
            var saleRepoMock = new SaleRepository(_context);
            var productRepoMock = new ProductRepository(_context);
            var branchRepoMock = new BranchRepository(_context);
            var cartRepoMock = new CartRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var adapter = new Mock<IDomainNotificationAdapter>();
            var branchExisting = BranchHandlerTestData.GetBranch();
            var productExisting = ProductHandlerTestData.GetProduct();
            var command = new CreateSaleCommand();
            command.BranchSaleId = branchExisting.Id;
            command.UserId = Guid.NewGuid();
            command.TotalSalePrice = 100.00m;
            var cartExisting = CartHandlerTestData.GetCart(command.UserId, branchExisting, productExisting, command.TotalSalePrice);
            command.CartId = cartExisting.Id;
            command.SaleItems = new List<CreateSaleItem>()
            {
                new CreateSaleItem
                {
                    ProductId = productExisting.Id,
                    ProductItemQuantity = 10,
                    UnitProductItemPrice = productExisting.Price.Value
                }
            };
            var sale = new Sale();
            sale.BranchSaleId = command.BranchSaleId;
            sale.UserId = command.UserId;
            sale.TotalSalePrice = new Money(command.TotalSalePrice);
            sale.AddSaleItems(command.SaleItems.Select(x =>
            {
                return new SaleItem
                {
                    ProductId = x.ProductId,
                    ProductItemQuantity = x.ProductItemQuantity,
                    UnitProductItemPrice = new Money(x.UnitProductItemPrice)
                };
            }).ToList());

            mapperMock.Setup(m => m.Map<CreateSaleCommand>(It.IsAny<Cart>())).Returns(command);
            mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
            mapperMock.Setup(m => m.Map<CreateSaleResult>(sale)).Returns(
                new CreateSaleResult(
                    Guid.NewGuid(), 
                    sale.UserId, 
                    sale.BranchSaleId, 
                    0, 
                    sale.TotalSalePrice.Value, 
                    sale.SaleStatus,
                    sale.SaleItems.Select(x => new CreateSaleItemResult(
                        x.Id, 
                        x.ProductId, 
                        x.ProductItemQuantity, 
                        x.UnitProductItemPrice.Value, 
                        x.DiscountAmount.Value, 
                        x.TotalSaleItemPrice.Value, 
                        x.TotalWithoutDiscount.Value, 
                        x.SaleItemStatus))));
            var handler = new CreateSaleHandler(saleRepoMock, productRepoMock, branchRepoMock, cartRepoMock, mapperMock.Object, adapter.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

    }
}
