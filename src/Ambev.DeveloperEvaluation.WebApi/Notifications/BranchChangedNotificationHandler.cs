using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class BranchChangedNotificationHandler : INotificationHandler<MediatRDomainNotification<BranchChangedEvent>>
    {
        private readonly ILogger<BranchChangedNotificationHandler> _logger;
        private readonly ICartRepository _cartRepository;

        public BranchChangedNotificationHandler(ILogger<BranchChangedNotificationHandler> logger, ICartRepository cartRepository)
        {
            _logger = logger;
            _cartRepository = cartRepository;
        }

        public async Task Handle(MediatRDomainNotification<BranchChangedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log BranchChangedEvent Id: {notification.Notification.BranchId}");
            await _cartRepository.UpdateBranchInCartAsync(notification.Notification.BranchId, notification.Notification.BranchName);
        }
    }
}
