using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_ValidId_DeletesProduct()
        {
            var productRepoMock = new Mock<IProductRepository>();
            var queryDbServiceMock = new Mock<IQueryDatabaseService>();
            var command = new DeleteProductCommand(Guid.NewGuid());
            productRepoMock.Setup(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new DeleteProductHandler(productRepoMock.Object, queryDbServiceMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
        }
    }
}
