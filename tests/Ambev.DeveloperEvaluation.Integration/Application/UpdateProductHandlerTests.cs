using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class UpdateProductHandlerTests
    {

        private readonly DefaultContext _context;

        public UpdateProductHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesProduct()
        {
            var repoMock = new ProductRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var notificationAdapter = new Mock<IDomainNotificationAdapter>();
            var cartRepoMock = new Mock<ICartRepository>();
            var productExisting = ProductHandlerTestData.GetProduct();
            var command = new UpdateProductCommand(productExisting.Id, "FakeProduct Updated", "FakeProductDescription Updated", 10.0m);
            var product = new Product(command.Id, command.Name, command.Description, command.Price);

            mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            mapperMock.Setup(m => m.Map<UpdateProductResult>(product)).Returns(new UpdateProductResult(command.Id,command.Name,command.Description,command.Price));

            var handler = new UpdateProductHandler(repoMock, mapperMock.Object, notificationAdapter.Object, cartRepoMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }

    }
}
