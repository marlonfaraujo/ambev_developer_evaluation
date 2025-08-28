using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleChangedNotificationHandler : INotificationHandler<MediatRDomainNotification<SaleChangedEvent>>
    {
        private readonly ILogger<SaleChangedNotificationHandler> _logger;

        public SaleChangedNotificationHandler(ILogger<SaleChangedNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MediatRDomainNotification<SaleChangedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleChangedEvent Id: {notification.Notification.SaleId}");
            return Task.CompletedTask;
        }
    }
}
