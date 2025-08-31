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
    public class SaleItemCancelledNotificationHandlerTests
    {
        [Fact]
        public async Task Handle_LogsInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SaleItemCancelledNotificationHandler>>();
            var mongoDbServiceMock = new Mock<IMongoDbService<ISaleModel>>();
            var queryDatabaseServiceMock = new Mock<IQueryDatabaseService>();
            var handler = new SaleItemCancelledNotificationHandler(loggerMock.Object, mongoDbServiceMock.Object, queryDatabaseServiceMock.Object);

            var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 1, 10, 0, 10, 10, "Cancelled");
            var saleItemCancelledEvent = new SaleItemCancelledEvent(saleItem.Id);
            var notification = new MediatRDomainNotification<SaleItemCancelledEvent>(saleItemCancelledEvent);

            // Act
            await handler.Handle(notification, CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SaleItemCancelledEvent")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
