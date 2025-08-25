using Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateBranchHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_UpdatesBranch()
        {
            // Arrange
            var repoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new UpdateBranchCommand(Guid.NewGuid(), "FakeName", "FakeDesc");
            var branch = new Branch(command.Id, command.Name, command.Description);
            var updatedBranch = new Branch(command.Id, "Updated", "UpdatedDesc");

            repoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(branch);
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Branch>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedBranch);
            mapperMock.Setup(m => m.Map<Branch>(command)).Returns(branch);
            mapperMock.Setup(m => m.Map<UpdateBranchResult>(updatedBranch)).Returns(new UpdateBranchResult(command.Id, command.Name, command.Description));

            var handler = new UpdateBranchHandler(repoMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            repoMock.Verify(r => r.UpdateAsync(It.IsAny<Branch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var repoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new UpdateBranchCommand(Guid.NewGuid(), "", "");
            var handler = new UpdateBranchHandler(repoMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_BranchNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var repoMock = new Mock<IBranchRepository>();
            var mapperMock = new Mock<IMapper>();
            var command = new UpdateBranchCommand(Guid.NewGuid(), "FakeName", "FakeDesc");

            repoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Branch)null);

            var handler = new UpdateBranchHandler(repoMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
