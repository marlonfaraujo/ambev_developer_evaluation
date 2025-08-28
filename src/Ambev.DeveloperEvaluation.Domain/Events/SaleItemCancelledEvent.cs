namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemCancelledEvent : IDomainNotification
    {
        public Guid SaleItemId { get; }
        public SaleItemCancelledEvent(Guid saleItemId)
        {
            SaleItemId = saleItemId;
        }
    }
}
