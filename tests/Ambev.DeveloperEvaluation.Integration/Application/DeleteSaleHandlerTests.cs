using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly DefaultContext _context;

        public DeleteSaleHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidId_DeletesSale()
        {
            var repoMock = new SaleRepository(_context);
            var saleExisting = SaleHandlerTestData.GetSale();
            var command = new DeleteSaleCommand(saleExisting.Id);

            var cancelCommand = new CancelSaleCommand(saleExisting.Id);
            var mapperMock = new Mock<IMapper>();
            var adapter = new Mock<IDomainNotificationAdapter>();
            var cancelHandler = new CancelSaleHandler(repoMock, mapperMock.Object, adapter.Object);
            var cancelResult = await cancelHandler.Handle(cancelCommand, CancellationToken.None);

            var deleteHandler = new DeleteSaleHandler(repoMock);
            var result = await deleteHandler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
        }

    }
}
