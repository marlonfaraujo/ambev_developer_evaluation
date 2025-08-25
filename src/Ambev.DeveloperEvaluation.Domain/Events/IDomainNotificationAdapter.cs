using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public interface IDomainNotificationAdapter
    {
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : IDomainNotification;
    }
}
