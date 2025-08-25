using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent : IDomainNotification
    {
        public Sale Sale { get; }
        public SaleCancelledEvent(Sale sale)
        {
            Sale = sale;
        }
    }
}
