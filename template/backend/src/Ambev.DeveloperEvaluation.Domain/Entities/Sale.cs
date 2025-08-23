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

        public decimal CalculateTotalPrice()
        {
            decimal itemDiscount = 0;

            foreach (var item in SaleItems)
            {
                decimal TotalItemPrice = item.UnitItemPrice * item.ItemQuantity;
                if (SaleItems.Any(x => item.ProductId == x.ProductId && x.ItemQuantity > 10 && x.ItemQuantity <= 20))
                {
                    itemDiscount = 0.2m;
                    TotalSalePrice += TotalItemPrice - (TotalItemPrice * itemDiscount);
                    continue;
                }

                if (SaleItems.Any(x => item.ProductId == x.ProductId && x.ItemQuantity > 4))
                {
                    itemDiscount = 0.1m;
                    TotalSalePrice += TotalItemPrice - (TotalItemPrice * itemDiscount);
                    continue;
                }
                TotalSalePrice += TotalItemPrice;
            }

            return TotalSalePrice;
        }

    }
}
