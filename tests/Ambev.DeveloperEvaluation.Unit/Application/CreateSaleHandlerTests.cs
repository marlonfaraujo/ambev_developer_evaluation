using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_CreatesSale()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var productRepoMock = new Mock<IProductRepository>();
            var branchRepoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var adapter = new Mock<IDomainNotificationAdapter>();
            var command = new CreateSaleCommand();
            command.BranchSaleId = Guid.NewGuid();
            command.UserId = Guid.NewGuid();
            command.TotalSalePrice = 100.00m;
            command.SaleItems = new List<SaleItem>()
            {
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductItemQuantity = 10,
                    UnitProductItemPrice = 2
                }
            };
            var sale = new Sale();
            sale.BranchSaleId = command.BranchSaleId;
            sale.UserId = command.UserId;
            sale.TotalSalePrice = command.TotalSalePrice;
            sale.AddSaleItems(command.SaleItems.ToList());

            saleRepoMock.Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ReturnsAsync(sale);
            mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
            mapperMock.Setup(m => m.Map<CreateSaleResult>(sale)).Returns(new CreateSaleResult(Guid.NewGuid(), 14, sale.TotalSalePrice, sale.SaleStatus, sale.SaleItems));

            var handler = new CreateSaleHandler(saleRepoMock.Object, productRepoMock.Object, branchRepoMock.Object, mapperMock.Object, adapter.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
