using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
    public class UpdateSaleHandlerTests
    {
        private readonly DefaultContext _context;

        public UpdateSaleHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesSale()
        {
            var saleRepoMock = new SaleRepository(_context);
            var productRepoMock = new ProductRepository(_context);
            var branchRepoMock = new BranchRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var command = new UpdateSaleCommand();
            var adapter = new Mock<IDomainNotificationAdapter>();
            var saleExisting = SaleHandlerTestData.GetSale();
            var productExisting = ProductHandlerTestData.GetProduct(); 
            var branchExisting = BranchHandlerTestData.GetBranch();
            command.Id = saleExisting.Id;
            command.BranchSaleId = branchExisting.Id;
            command.SaleItems = new List<SaleItem>()
            {
                new SaleItem
                {
                    ProductId = productExisting.Id,
                    ProductItemQuantity = 14,
                    UnitProductItemPrice = productExisting.Price
                }
            };
            var sale = new Sale();
            sale.Id = command.Id;
            sale.BranchSaleId = command.BranchSaleId;
            sale.UserId = saleExisting.UserId;
            sale.TotalSalePrice = saleExisting.TotalSalePrice;
            sale.AddSaleItems(command.SaleItems.ToList());

            mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
            mapperMock.Setup(m => m.Map<UpdateSaleResult>(sale)).Returns(
                new UpdateSaleResult(
                    sale.Id, 
                    sale.SaleNumber, 
                    sale.SaleDate, 
                    sale.UserId, 
                    sale.TotalSalePrice.Value, 
                    sale.BranchSaleId, 
                    sale.SaleStatus, 
                    sale.SaleItems.Select(x => 
                    new UpdateSaleItemResult(
                        x.Id,
                        x.ProductId,
                        x.ProductItemQuantity,
                        x.UnitProductItemPrice.Value,
                        x.DiscountAmount.Value,
                        x.TotalSaleItemPrice.Value,
                        x.TotalWithoutDiscount.Value,
                        x.SaleItemStatus
                        ))));
            var handler = new UpdateSaleHandler(saleRepoMock, productRepoMock, branchRepoMock, mapperMock.Object, adapter.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

    }
}
