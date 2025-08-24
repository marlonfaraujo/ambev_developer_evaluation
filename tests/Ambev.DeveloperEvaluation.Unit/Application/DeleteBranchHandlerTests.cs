using Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteBranchHandlerTests
    {

        [Fact]
        public async Task Handle_ValidId_DeletesBranch()
        {
            var branchRepoMock = new Mock<IBranchRepository>();
            var queryDbServiceMock = new Mock<IQueryDatabaseService>();
            var command = new DeleteBranchCommand(Guid.NewGuid());
            branchRepoMock.Setup(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new DeleteBranchHandler(branchRepoMock.Object, queryDbServiceMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
        }
    }
}
