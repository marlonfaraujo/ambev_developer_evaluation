using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleHandlerTests
    {
        [Fact]
        public async Task Handle_ValidId_ReturnsSale()
        {
            var repoMock = new Mock<ISaleRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new GetSaleQuery(Guid.NewGuid());
            var sale = new Sale();
            sale.Id = command.Id;
            sale.SetSaleNumber(55);
            sale.TotalSalePrice = 500m;
            sale.AddSaleItems(new List<SaleItem>() 
                { 
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        ProductItemQuantity = 11,
                        UnitProductItemPrice = 10
                    }
            });

            repoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);
            mapperMock.Setup(m => m.Map<GetSaleResult>(sale)).Returns(new GetSaleResult(sale.Id, sale.SaleNumber, sale.TotalSalePrice, sale.SaleStatus, sale.SaleItems));

            var handler = new GetSaleHandler(repoMock.Object, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
