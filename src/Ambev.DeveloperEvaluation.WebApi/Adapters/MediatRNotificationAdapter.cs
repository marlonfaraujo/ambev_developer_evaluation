using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.WebApi.Notifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Adapters
{
    public class MediatRNotificationAdapter : IDomainNotificationAdapter
    {
        private readonly IMediator _mediator;

        public MediatRNotificationAdapter(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : IDomainNotification
        {
           await _mediator.Publish(new MediatRDomainNotification<TNotification>(notification), cancellationToken);
        }
    }
}
