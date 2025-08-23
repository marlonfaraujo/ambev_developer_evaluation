using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string Id { get; }
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
            SaleItems = GetSaleItemsGroupedByProductId(saleItems);
        }

        public decimal CalculateTotalSalePriceWithItems()
        {
            if (HasMaxQuantityProductItems(SaleItems)) throw new MaxQuantityProductItemsException($"The maximum quantity of product items is {MaxQuantityProductItemsSpecification.MAX_ITEMS_PER_PRODUCT}.");
           
            TotalSalePrice = SaleItems.Sum(x => x.CalculateTotalSaleItemPrice());
            return TotalSalePrice;
        }

        public IEnumerable<SaleItem> GetSaleItemsGroupedByProductId(IEnumerable<SaleItem> saleItems)
        {
            if (saleItems == null || !saleItems.Any())
            {
                return Enumerable.Empty<SaleItem>();
            }
            return saleItems
                .GroupBy(x => x.ProductId)
                .Select(g =>
                {
                    var saleItem = g.First();
                    saleItem.SetItemQuantity(g.Sum(p => p.ProductItemQuantity));
                    return saleItem;
                })
                .ToList();
        }

        private bool HasMaxQuantityProductItems(IEnumerable<SaleItem> saleItems)
        {
            var spec = new MaxQuantityProductItemsSpecification();
            return spec.IsSatisfiedBy(saleItems);
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
