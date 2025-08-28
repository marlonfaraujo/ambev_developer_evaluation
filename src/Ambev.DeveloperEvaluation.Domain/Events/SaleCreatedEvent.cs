using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : IDomainNotification
    {
        public Guid SaleId { get; }
        public SaleCreatedEvent(Guid saleId)
        {
            SaleId = saleId;
        }
    }
}
