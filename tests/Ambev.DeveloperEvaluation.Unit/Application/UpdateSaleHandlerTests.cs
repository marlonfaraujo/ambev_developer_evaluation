using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateSaleHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_UpdatesSale()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var productRepoMock = new Mock<IProductRepository>();
            var branchRepoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new UpdateSaleCommand();
            command.Id = Guid.NewGuid();
            command.BranchSaleId = Guid.NewGuid();
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
            sale.Id = command.Id;
            sale.BranchSaleId = command.BranchSaleId;
            sale.UserId = Guid.NewGuid();
            sale.AddSaleItems(command.SaleItems.ToList());
            sale.SetSaleNumber(8);
            sale.SaleDate = DateTime.Now;
            sale.TotalSalePrice = 100m;
            sale.UpdateSale();

            saleRepoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);
            saleRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ReturnsAsync(sale);
            mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
            mapperMock.Setup(m => m.Map<UpdateSaleResult>(sale)).Returns(new UpdateSaleResult(sale.Id, sale.SaleNumber, sale.SaleDate, sale.UserId, sale.TotalSalePrice, sale.BranchSaleId, sale.SaleStatus, sale.SaleItems));

            var handler = new UpdateSaleHandler(saleRepoMock.Object, productRepoMock.Object, branchRepoMock.Object, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
