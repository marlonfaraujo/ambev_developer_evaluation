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
            SaleItems = SaleItem.GetSaleItemsGroupedByProductId(SaleItems);
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

    }
}
