namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent : IDomainNotification
    {
        public Guid SaleId { get; }
        public SaleCancelledEvent(Guid saleId)
        {
            SaleId = saleId;
        }
    }
}
