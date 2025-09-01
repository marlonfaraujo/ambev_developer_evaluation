using Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application
{
    public class UpdateBranchHandlerTests
    {
        private readonly DefaultContext _context;

        public UpdateBranchHandlerTests()
        {
            _context = new DatabaseInMemory().Context;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesBranch()
        {
            // Arrange
            var repoMock = new BranchRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var notificationAdapter = new Mock<IDomainNotificationAdapter>();
            var branchExisting = BranchHandlerTestData.GetBranch();
            var command = new UpdateBranchCommand(branchExisting.Id, "FakeName Updated", "FakeDesc Updated");
            var branch = new Branch(command.Id, command.Name, command.Description);

            mapperMock.Setup(m => m.Map<Branch>(command)).Returns(branch);
            mapperMock.Setup(m => m.Map<UpdateBranchResult>(branch)).Returns(new UpdateBranchResult(command.Id, command.Name, command.Description));

            var handler = new UpdateBranchHandler(repoMock, mapperMock.Object, notificationAdapter.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var repoMock = new BranchRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var notificationAdapter = new Mock<IDomainNotificationAdapter>();
            var command = new UpdateBranchCommand(Guid.NewGuid(), "", "");
            var handler = new UpdateBranchHandler(repoMock, mapperMock.Object, notificationAdapter.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_BranchNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var repoMock = new BranchRepository(_context);
            var mapperMock = new Mock<IMapper>();
            var notificationAdapter = new Mock<IDomainNotificationAdapter>();
            var command = new UpdateBranchCommand(Guid.NewGuid(), "FakeName", "FakeDesc");

            var handler = new UpdateBranchHandler(repoMock, mapperMock.Object, notificationAdapter.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
