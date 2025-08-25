using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetBranchHandlerTests
    {

        [Fact]
        public async Task Handle_ValidId_ReturnsBranch()
        {
            var repoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new GetBranchQuery(Guid.NewGuid());
            var branch = new Branch(command.Id, "FakeName", "FakeDesc");

            repoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(branch);
            mapperMock.Setup(m => m.Map<GetBranchResult>(branch)).Returns(new GetBranchResult(branch.Id, branch.Name, branch.Description));

            var handler = new GetBranchHandler(repoMock.Object, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
