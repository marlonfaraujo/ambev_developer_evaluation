
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateProductHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_CreatesProduct()
        {
            var repoMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new CreateProductCommand("FakeProduct", "FakeProductDescription", 10.0m);
            var product = new Product(Guid.NewGuid(), command.Name, command.Description, command.Price);

            repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            mapperMock.Setup(m => m.Map<CreateProductResult>(product)).Returns(new CreateProductResult(product.Id, product.Name, product.Description, product.Price));

            var handler = new CreateProductHandler(repoMock.Object, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
