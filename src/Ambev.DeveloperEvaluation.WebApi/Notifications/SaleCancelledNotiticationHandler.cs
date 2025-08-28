using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleCancelledNotiticationHandler : INotificationHandler<MediatRDomainNotification<SaleCancelledEvent>>
    {
        private readonly ILogger<SaleCancelledNotiticationHandler> _logger;

        public SaleCancelledNotiticationHandler(ILogger<SaleCancelledNotiticationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MediatRDomainNotification<SaleCancelledEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleCancelledEvent Id: {notification.Notification.SaleId}");
            return Task.CompletedTask;
        }
    }
}
