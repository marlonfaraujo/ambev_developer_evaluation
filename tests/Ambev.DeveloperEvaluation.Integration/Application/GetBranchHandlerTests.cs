using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
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
    public class GetBranchHandlerTests
    {
        private readonly DefaultContext _context;

        public GetBranchHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsBranch()
        {
            var repoMock = new BranchRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var branchExisting = BranchHandlerTestData.GetBranch();
            var command = new GetBranchQuery(branchExisting.Id);
            var branch = new Branch();
            branch.Id = command.Id;
            branch.Name = branchExisting.Name;
            branch.Description = branchExisting.Description;

            mapperMock.Setup(m => m.Map<Branch>(command)).Returns(branchExisting);
            mapperMock.Setup(m => m.Map<GetBranchResult>(It.IsAny<Branch>())).Returns(new GetBranchResult(branch.Id, branch.Name, branch.Description));
            var handler = new GetBranchHandler(repoMock, mapperMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

    }
}
