namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemQuantity { get; set; }
        public decimal UnitProductItemPrice { get; set; }
        public decimal DiscountAmountApplied { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalSaleItemPrice { get; set; }
        public decimal TotalProductItemPriceWithoutDiscount { get; set; }
        public string SaleItemStatus { get; set; }
        public SaleItem()
        {
            DiscountAmountApplied = 0;
            TotalSaleItemPrice = 0;
        }

        public static IEnumerable<SaleItem> GetSaleItemsGroupedByProductId(IEnumerable<SaleItem> saleItems)
        {
            if (saleItems == null || !saleItems.Any())
            {
                return Enumerable.Empty<SaleItem>();
            }
            return saleItems
                .GroupBy(x => x.ProductId)
                .Select(g => new SaleItem
                {
                    SaleId = g.First().SaleId,
                    ProductId = g.Key,
                    ProductItemQuantity = g.Sum(p => p.ProductItemQuantity),
                    UnitProductItemPrice = g.First().UnitProductItemPrice,
                    DiscountAmountApplied = g.First().DiscountAmountApplied,
                    TotalDiscountAmount = g.First().TotalDiscountAmount,
                    TotalSaleItemPrice = g.First().TotalSaleItemPrice,
                    TotalProductItemPriceWithoutDiscount = g.First().TotalProductItemPriceWithoutDiscount,
                    SaleItemStatus = g.First().SaleItemStatus
                })
                .ToList();
        }

        public decimal CalculateTotalSaleItemPrice()
        {
            TotalProductItemPriceWithoutDiscount = UnitProductItemPrice * ProductItemQuantity;
            TotalDiscountAmount = CalculateTotalDiscountAmount(TotalProductItemPriceWithoutDiscount);
            TotalSaleItemPrice = TotalProductItemPriceWithoutDiscount - TotalDiscountAmount;
            return TotalSaleItemPrice;
        }

        private decimal CalculateTotalDiscountAmount(decimal totalProductItemPrice)
        {
            decimal totalDiscountAmount = 0;
            if (ProductItemQuantity > 10 && ProductItemQuantity <= 20)
            {
                DiscountAmountApplied = 0.2m;
                totalDiscountAmount = totalProductItemPrice * DiscountAmountApplied;
                return totalDiscountAmount;
            }
            if (ProductItemQuantity > 4)
            {
                DiscountAmountApplied = 0.1m;
                totalDiscountAmount = totalProductItemPrice * DiscountAmountApplied;
                return totalDiscountAmount;
            }
            return totalDiscountAmount;
        }
    }
}
