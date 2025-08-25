using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.WebApi.Notifications;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Notification
{
    public class SaleChangedNotificationHandlerTests
    {
        [Fact]
        public async Task Handle_LogsInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SaleChangedNotificationHandler>>();
            var handler = new SaleChangedNotificationHandler(loggerMock.Object);

            var notification = new MediatRDomainNotification<SaleChangedEvent>(new Sale().UpdateSale());

            // Act
            await handler.Handle(notification, CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SaleChangedEvent")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
