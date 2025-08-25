using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : IDomainNotification
    {
        public Sale Sale { get; }
        public SaleCreatedEvent(Sale sale)
        {
            Sale = sale;
        }
    }
}
