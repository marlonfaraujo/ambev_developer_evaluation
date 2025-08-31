using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.WebApi.Notifications;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Notification
{
    public class SaleCancelledNotiticationHandlerTests
    {
        [Fact]
        public async Task Handle_LogsInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SaleCancelledNotiticationHandler>>();
            var mongoDbServiceMock = new Mock<IMongoDbService<ISaleModel>>();
            var queryDatabaseServiceMock = new Mock<IQueryDatabaseService>();
            var handler = new SaleCancelledNotiticationHandler(loggerMock.Object, mongoDbServiceMock.Object, queryDatabaseServiceMock.Object);

            var notification = new MediatRDomainNotification<SaleCancelledEvent>(new Sale().CancelSale());

            // Act
            await handler.Handle(notification, CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SaleCancelledEvent")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
