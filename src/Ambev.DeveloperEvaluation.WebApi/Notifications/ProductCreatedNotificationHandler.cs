using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class ProductCreatedNotificationHandler : INotificationHandler<MediatRDomainNotification<ProductCreatedEvent>>
    {
        private readonly ILogger<ProductCreatedNotificationHandler> _logger;
        private readonly ICacheDatabase _cacheDatabase;

        public ProductCreatedNotificationHandler(ILogger<ProductCreatedNotificationHandler> logger, ICacheDatabase cacheDatabase)
        {
            _logger = logger;
            _cacheDatabase = cacheDatabase;
        }

        public async Task Handle(MediatRDomainNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log ProductCreatedEvent Id: {notification.Notification.ProductId}");
            await _cacheDatabase.RemoverAsync("products-cache", cancellationToken);
        }
    }
}
