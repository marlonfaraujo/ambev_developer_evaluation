using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class MediatRDomainNotification<TNotification> : INotification where TNotification : IDomainNotification
    {
        public TNotification Notification { get; }

        public MediatRDomainNotification(TNotification notification)
        {
            Notification = notification;
        }

    }
}
