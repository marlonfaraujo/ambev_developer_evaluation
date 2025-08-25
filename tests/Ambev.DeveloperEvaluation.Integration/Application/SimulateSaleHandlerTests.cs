using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
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
    public class SimulateSaleHandlerTests
    {
        private readonly DefaultContext _context;

        public SimulateSaleHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsSimulationResult()
        {
            var branchRepoMock = new BranchRepository(_context);
            var productRepoMock = new ProductRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var simulateQuery = new SimulateSaleQuery();
            var branchExisting = BranchHandlerTestData.GetBranch();
            var productExisting = ProductHandlerTestData.GetProduct();
            simulateQuery.BranchSaleId = branchExisting.Id;
            simulateQuery.SaleItems = new List<SaleItem>()
            {
                new SaleItem
                {
                    ProductId = productExisting.Id,
                    UnitProductItemPrice = productExisting.Price,
                    ProductItemQuantity = 10
                }
            };

            var sale = new Sale
            {
                BranchSaleId = branchExisting.Id,
                TotalSalePrice = 0
            };
            sale.AddSaleItems(simulateQuery.SaleItems.ToList());
            // Setup repository and mapper mocks as needed for simulation
            mapperMock.Setup(m => m.Map<Sale>(simulateQuery)).Returns(sale);
            mapperMock.Setup(m => m.Map<SimulateSaleResult>(sale)).Returns(new SimulateSaleResult { BranchSaleId = branchExisting.Id, UserId = sale.UserId, TotalSalePrice = sale.TotalSalePrice, SaleItems = sale.SaleItems});
            var handler = new SimulateSaleHandler(productRepoMock, branchRepoMock, mapperMock.Object);
            var result = await handler.Handle(simulateQuery, CancellationToken.None);
            Assert.NotNull(result);
        }

    }
}
