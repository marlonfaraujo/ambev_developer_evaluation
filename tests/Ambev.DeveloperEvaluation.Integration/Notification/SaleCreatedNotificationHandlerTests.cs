using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.NoSql;
using Ambev.DeveloperEvaluation.WebApi.Notifications;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Notification
{
    public class SaleCreatedNotificationHandlerTests
    {
        [Fact]
        public async Task Handle_LogsInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SaleCreatedNotificationHandler>>();
            var mongoDbContextMock = new Mock<MongoDbContext>("mongodb://developer:ev%40luAt10n@localhost:27017/?authSource=admin", "test_db_name");
            var mongoDbServiceMock = new Mock<MongoDbService<ISaleModel>>(mongoDbContextMock.Object, "sales");
            var queryDatabaseServiceMock = new Mock<Ambev.DeveloperEvaluation.Application.Services.IQueryDatabaseService>();
            var handler = new SaleCreatedNotificationHandler(loggerMock.Object, mongoDbServiceMock.Object, queryDatabaseServiceMock.Object);

            var notification = new MediatRDomainNotification<SaleCreatedEvent>(new Sale().CreateSaleEvent());

            // Act
            await handler.Handle(notification, CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SaleCreatedEvent")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
