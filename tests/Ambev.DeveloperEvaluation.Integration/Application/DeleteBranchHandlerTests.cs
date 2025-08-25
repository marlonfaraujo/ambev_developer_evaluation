using Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class DeleteBranchHandlerTests
    {
        private readonly DefaultContext _context;

        public DeleteBranchHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidId_DeletesBranch()
        {
            var branchRepoMock = new BranchRepository(_context);
            var queryDbServiceMock = new Mock<IQueryDatabaseService>();
            var branchExisting = BranchHandlerTestData.GetBranch();
            var command = new DeleteBranchCommand(branchExisting.Id);

            var handler = new DeleteBranchHandler(branchRepoMock, queryDbServiceMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
        }

    }
}
