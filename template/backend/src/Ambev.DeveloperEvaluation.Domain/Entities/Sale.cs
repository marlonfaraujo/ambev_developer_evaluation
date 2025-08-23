using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

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
        public IEnumerable<SaleItem> SaleItems { get; private set; }

        public Sale()
        {
            SaleItems = new List<SaleItem>();
            SaleStatus = SaleStatusEnum.Created.ToString();
        }

        public void AddSaleItems(IEnumerable<SaleItem> saleItems)
        {
            SaleItems = saleItems;
        }

        public void CancelSale()
        {
            SaleStatus = SaleStatusEnum.Cancelled.ToString();
        }

        public void UpdateSale()
        {
            SaleStatus = SaleStatusEnum.Modified.ToString();
        }
    }
}
