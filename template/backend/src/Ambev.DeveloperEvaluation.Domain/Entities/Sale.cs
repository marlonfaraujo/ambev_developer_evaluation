using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalSalePrice { get; set; }
        public int BranchSaleId { get; set; }
        public string SaleStatus { get; set; }
        public IEnumerable<SaleItem> SaleItems { get; set; }

        public Sale()
        {
        }

        public decimal CalculateTotalSalePriceWithItems()
        {
            SaleItems = GetSaleItemsGroupedByProductId();
            if (HasMaxQuantityProductItems(SaleItems)) throw new MaxQuantityProductItemsException($"The maximum quantity of product items is {MaxQuantityProductItemsSpecification.MAX_ITEMS_PER_PRODUCT}.");
           
            TotalSalePrice = SaleItems.Sum(x => x.CalculateTotalSaleItemPrice());
            return TotalSalePrice;
        }

        public IEnumerable<SaleItem> GetSaleItemsGroupedByProductId()
        {
            if (SaleItems == null || !SaleItems.Any())
            {
                return Enumerable.Empty<SaleItem>();
            }
            return SaleItems
                .GroupBy(x => x.ProductId)
                .Select(g => new SaleItem
                {
                    SaleId = g.First().SaleId,
                    ProductId = g.Key,
                    ProductItemQuantity = g.Sum(p => p.ProductItemQuantity),
                    UnitProductItemPrice = g.First().UnitProductItemPrice,
                    DiscountAmount = g.First().DiscountAmount,
                    TotalSaleItemPrice = g.First().TotalSaleItemPrice,
                    TotalWithoutDiscount = g.First().TotalWithoutDiscount,
                    SaleItemStatus = g.First().SaleItemStatus
                })
                .ToList();
        }

        private bool HasMaxQuantityProductItems(IEnumerable<SaleItem> saleItems)
        {
            var spec = new MaxQuantityProductItemsSpecification();
            return spec.IsSatisfiedBy(saleItems);
        }

    }
}
