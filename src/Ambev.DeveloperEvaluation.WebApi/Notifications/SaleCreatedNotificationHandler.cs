using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleCreatedNotificationHandler : INotificationHandler<MediatRDomainNotification<SaleCreatedEvent>>
    {
        private readonly ILogger<SaleCreatedNotificationHandler> _logger;

        public SaleCreatedNotificationHandler(ILogger<SaleCreatedNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MediatRDomainNotification<SaleCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleCreatedEvent Id: {notification.Notification.Sale.Id}");
            return Task.CompletedTask;
        }
    }
}
