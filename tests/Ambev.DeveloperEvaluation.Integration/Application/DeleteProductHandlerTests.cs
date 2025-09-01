using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class DeleteProductHandlerTests
    {
        private readonly DefaultContext _context;

        public DeleteProductHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidId_DeletesProduct()
        {
            var productRepoMock = new ProductRepository(_context);
            var cartRepoMock = new CartRepository(_context);
            var queryDbServiceMock = new Mock<IQueryDatabaseService>();
            var notificationMock = new Mock<IDomainNotificationAdapter>();
            var productExisting = ProductHandlerTestData.GetProduct();
            var command = new DeleteProductCommand(productExisting.Id);

            var handler = new DeleteProductHandler(productRepoMock, queryDbServiceMock.Object, cartRepoMock, notificationMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
        }

    }
}
