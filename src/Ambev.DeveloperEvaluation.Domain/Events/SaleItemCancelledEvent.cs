using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemCancelledEvent : IDomainNotification
    {
        public SaleItem SaleItem { get; }
        public SaleItemCancelledEvent(SaleItem saleItem)
        {
            SaleItem = saleItem;
        }
    }
}
