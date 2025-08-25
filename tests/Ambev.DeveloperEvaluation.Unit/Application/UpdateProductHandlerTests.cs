using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateProductHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_UpdatesProduct()
        {
            var repoMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new UpdateProductCommand(Guid.NewGuid(), "FakeProduct", "FakeProductDescription", 10.0m);
            var product = new Product(command.Id, command.Name, command.Description, command.Price);

            repoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            mapperMock.Setup(m => m.Map<UpdateProductResult>(product)).Returns(new UpdateProductResult(command.Id,command.Name,command.Description,command.Price));

            var handler = new UpdateProductHandler(repoMock.Object, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
