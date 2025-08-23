using Ambev.DeveloperEvaluation.Domain.Exceptions;

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

        private const int MAX_ITEMS_PER_PRODUCT = 20;

        public Sale()
        {
        }

        public decimal CalculateTotalSalePriceWithItems()
        {
            SaleItems = SaleItem.GetSaleItemsGroupedByProductId(SaleItems);
            if (HasMaxQuantityProductItems(SaleItems))
            {
                throw new MaxQuantityProductItemsException($"The maximum quantity of product items is {MAX_ITEMS_PER_PRODUCT}.");
            }
            foreach (var item in SaleItems)
            {
                if (SaleItems.Any(x => item.ProductId == x.ProductId))
                {
                    TotalSalePrice += item.CalculateTotalSaleItemPrice();
                    continue;
                }
                TotalSalePrice += item.UnitProductItemPrice * item.ProductItemQuantity;
            }
            return TotalSalePrice;
        }

        private bool HasMaxQuantityProductItems(IEnumerable<SaleItem> saleItems)
        {
            if (SaleItems.Any(x => x.ProductItemQuantity > MAX_ITEMS_PER_PRODUCT))
            {
                return true;
            }
            return false;
        }

    }
}
