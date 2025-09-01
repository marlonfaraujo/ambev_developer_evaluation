using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class ProductDeletedNotificationHandler : INotificationHandler<MediatRDomainNotification<ProductDeletedEvent>>
    {
        private readonly ILogger<ProductDeletedNotificationHandler> _logger;
        private readonly ICacheDatabase _cacheDatabase;

        public ProductDeletedNotificationHandler(ILogger<ProductDeletedNotificationHandler> logger, ICacheDatabase cacheDatabase)
        {
            _logger = logger;
            _cacheDatabase = cacheDatabase;
        }

        public async Task Handle(MediatRDomainNotification<ProductDeletedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log ProductDeletedEvent Id: {notification.Notification.ProductId}");
            await _cacheDatabase.RemoverAsync("products-cache", cancellationToken);
        }
    }
}
