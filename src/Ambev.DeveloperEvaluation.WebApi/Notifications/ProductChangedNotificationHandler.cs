using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class ProductChangedNotificationHandler : INotificationHandler<MediatRDomainNotification<ProductChangedEvent>>
    {
        private readonly ILogger<ProductChangedNotificationHandler> _logger;
        private readonly IQueryDatabaseService _queryDatabaseService;

        public ProductChangedNotificationHandler(ILogger<ProductChangedNotificationHandler> logger, IQueryDatabaseService queryDatabaseService)
        {
            _logger = logger;
            _queryDatabaseService = queryDatabaseService;
        }

        public async Task Handle(MediatRDomainNotification<ProductChangedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log ProductChangedEvent Id: {notification.Notification.ProductId}");
            await _queryDatabaseService.UpdateProductInCart(notification.Notification.ProductId, notification.Notification.ProductName);
        }
    }
}
