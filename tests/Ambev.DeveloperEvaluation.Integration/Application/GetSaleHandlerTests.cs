using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class GetSaleHandlerTests
    {
        private readonly DefaultContext _context;

        public GetSaleHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsSale()
        {
            var repoMock = new SaleRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var saleExisting = SaleHandlerTestData.GetSale();
            var command = new GetSaleQuery(saleExisting.Id);
            var sale = new Sale();
            sale.Id = command.Id;
            sale.SetSaleNumber(saleExisting.SaleNumber);
            sale.TotalSalePrice = saleExisting.TotalSalePrice;
            sale.AddSaleItems(saleExisting.SaleItems.ToList());

            mapperMock.Setup(m => m.Map<GetSaleResult>(It.IsAny<Sale>())).Returns(
                new GetSaleResult(
                    sale.Id, 
                    sale.SaleNumber, 
                    sale.TotalSalePrice.Value, 
                    saleExisting.SaleStatus, 
                    sale.SaleItems.Select(x => new GetSaleItemResult(
                        x.Id,
                        x.ProductId,
                        x.ProductItemQuantity,
                        x.UnitProductItemPrice.Value,
                        x.DiscountAmount.Value,
                        x.TotalSaleItemPrice.Value,
                        x.TotalWithoutDiscount.Value,
                        x.SaleItemStatus
                        ))));
            var handler = new GetSaleHandler(repoMock, mapperMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }
    }
}
