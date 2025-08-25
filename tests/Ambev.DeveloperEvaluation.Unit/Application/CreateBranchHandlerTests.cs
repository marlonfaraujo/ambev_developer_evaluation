using Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateBranchHandlerTests
    {

        [Fact]
        public async Task Handle_ValidCommand_CreatesBranch()
        {
            var repoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new CreateBranchCommand("FakeName", "FakeDesc");
            var branch = new Branch(Guid.NewGuid(), command.Name, command.Description);

            repoMock.Setup(r => r.CreateAsync(It.IsAny<Branch>(), It.IsAny<CancellationToken>())).ReturnsAsync(branch);
            mapperMock.Setup(m => m.Map<Branch>(command)).Returns(branch);
            mapperMock.Setup(m => m.Map<CreateBranchResult>(branch)).Returns(new CreateBranchResult(branch.Id, branch.Name, branch.Description));

            var handler = new CreateBranchHandler(repoMock.Object, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            repoMock.Verify(r => r.CreateAsync(It.IsAny<Branch>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
