
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class CreateProductHandlerTests
    {
        private readonly DefaultContext _context;

        public CreateProductHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesProduct()
        {
            var repoMock = new ProductRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var notificationMock = new Mock<IDomainNotificationAdapter>();
            var command = new CreateProductCommand("FakeProduct", "FakeProductDescription", 10.0m);
            var product = new Product(Guid.NewGuid(), command.Name, command.Description, command.Price);

            mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            mapperMock.Setup(m => m.Map<CreateProductResult>(product)).Returns(new CreateProductResult(product.Id, product.Name, product.Description, product.Price.Value));

            var handler = new CreateProductHandler(repoMock, mapperMock.Object, notificationMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
