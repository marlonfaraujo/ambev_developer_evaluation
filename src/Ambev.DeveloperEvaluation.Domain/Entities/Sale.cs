using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public int SaleNumber { get; private set; }
        public DateTime SaleDate { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalSalePrice { get; set; }
        public Guid BranchSaleId { get; set; }
        public string SaleStatus { get; private set; }
        public List<SaleItem> SaleItems { get; private set; }

        public Sale()
        {
            SaleDate = DateTime.Now;
            SaleItems = new List<SaleItem>();
            SaleStatus = SaleStatusEnum.Created.ToString();
        }

        public void AddSaleItems(List<SaleItem> saleItems)
        {
            SaleItems = saleItems;
        }

        public SaleCreatedEvent CreateSaleEvent()
        {
            return new SaleCreatedEvent(this);
        }

        public SaleCancelledEvent CancelSale()
        {
            SaleStatus = SaleStatusEnum.Cancelled.ToString();
            return new SaleCancelledEvent(this);
        }

        public IEnumerable<SaleItemCancelledEvent> CancelSaleItems()
        {
            var events = new List<SaleItemCancelledEvent>();
            if (SaleItems != null && SaleItems.Any())
            {
                foreach (var item in SaleItems)
                {
                    if (item.SaleItemStatus != SaleItemStatusEnum.Cancelled.ToString())
                    {
                        events.Add(item.CancelItem());
                    }
                        
                }
            }
            return events;
        }

        public SaleChangedEvent UpdateSale()
        {
            SaleStatus = SaleStatusEnum.Modified.ToString();
            return new SaleChangedEvent(this);
        }

        public void SetSaleNumber(int saleNumber)
        {
            SaleNumber = saleNumber;
        }
    }
}
