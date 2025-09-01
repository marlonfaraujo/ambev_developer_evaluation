namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleChangedEvent : IDomainNotification
    {
        public Guid SaleId { get; }
        public SaleChangedEvent(Guid saleId)
        {
            SaleId = saleId;
        }
    }
}
