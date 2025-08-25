using Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class CreateBranchHandlerTests
    {
        private readonly DefaultContext _context;

        public CreateBranchHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesBranch()
        {
            var repoMock = new BranchRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var command = new CreateBranchCommand("FakeName", "FakeDesc");
            var branch = new Branch(Guid.NewGuid(), command.Name, command.Description);

            mapperMock.Setup(m => m.Map<Branch>(command)).Returns(branch);
            mapperMock.Setup(m => m.Map<CreateBranchResult>(branch)).Returns(new CreateBranchResult(branch.Id, branch.Name, branch.Description));

            var handler = new CreateBranchHandler(repoMock, mapperMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
