using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {

        [Fact]
        public async Task Handle_ValidId_DeletesSale()
        {
            var repoMock = new Mock<ISaleRepository>();
            var command = new DeleteSaleCommand(Guid.NewGuid());
            repoMock.Setup(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new DeleteSaleHandler(repoMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
        }
    }
}
