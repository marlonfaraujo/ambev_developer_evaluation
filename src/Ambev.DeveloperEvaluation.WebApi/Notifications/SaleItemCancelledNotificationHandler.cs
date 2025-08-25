using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleItemCancelledNotificationHandler : INotificationHandler<MediatRDomainNotification<SaleItemCancelledEvent>>
    {
        private readonly ILogger<SaleItemCancelledNotificationHandler> _logger;

        public SaleItemCancelledNotificationHandler(ILogger<SaleItemCancelledNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MediatRDomainNotification<SaleItemCancelledEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleItemCancelledEvent Id: {notification.Notification.SaleItem.Id}");
            return Task.CompletedTask;
        }
    }
}
