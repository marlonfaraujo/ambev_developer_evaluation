using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
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
    public class GetProductHandlerTests
    {
        private readonly DefaultContext _context;

        public GetProductHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsProduct()
        {
            var repoMock = new ProductRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var productExisting = ProductHandlerTestData.GetProduct();
            var productQuery = new GetProductQuery(productExisting.Id);

            mapperMock.Setup(m => m.Map<GetProductResult>(It.IsAny<Product>())).Returns(new GetProductResult(productExisting.Id, productExisting.Name, productExisting.Description, productExisting.Price));
            var handler = new GetProductHandler(repoMock, mapperMock.Object);
            var result = await handler.Handle(productQuery, CancellationToken.None);
            Assert.NotNull(result);
        }

    }
}
