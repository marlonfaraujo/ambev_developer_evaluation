using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetProductHandlerTests
    {
        [Fact]
        public async Task Handle_ValidId_ReturnsProduct()
        {
            var repoMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();
            var productQuery = new GetProductQuery(Guid.NewGuid());
            var product = new Product(productQuery.Id, "FakeProduct", "FakeProductDescription", 10.0m);

            repoMock.Setup(r => r.GetByIdAsync(productQuery.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mapperMock.Setup(m => m.Map<GetProductResult>(product)).Returns(new GetProductResult(product.Id, product.Name, product.Description, product.Price));

            var handler = new GetProductHandler(repoMock.Object, mapperMock.Object);

            var result = await handler.Handle(productQuery, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
