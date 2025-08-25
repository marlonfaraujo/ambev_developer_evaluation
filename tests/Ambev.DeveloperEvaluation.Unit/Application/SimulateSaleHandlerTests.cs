using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class SimulateSaleHandlerTests
    {
        [Fact]
        public async Task Handle_ValidQuery_ReturnsSimulationResult()
        {
            var branchRepoMock = new Mock<IBranchRepository>();
            var productRepoMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();
            var simulateQuery = new SimulateSaleQuery();
            simulateQuery.BranchSaleId = Guid.NewGuid();
            simulateQuery.SaleItems = new List<SaleItem>()
            {
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    UnitProductItemPrice = 15.00m,
                    ProductItemQuantity = 10
                }
            };

            // Setup repository and mapper mocks as needed for simulation
            mapperMock.Setup(m => m.Map<SimulateSaleResult>(It.IsAny<object>())).Returns(new SimulateSaleResult());

            var handler = new SimulateSaleHandler(productRepoMock.Object, branchRepoMock.Object, mapperMock.Object);

            var result = await handler.Handle(simulateQuery, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
