using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleChangedEvent : IDomainNotification
    {
        public Sale Sale { get; }
        public SaleChangedEvent(Sale sale)
        {
            Sale = sale;
        }
    }
}
