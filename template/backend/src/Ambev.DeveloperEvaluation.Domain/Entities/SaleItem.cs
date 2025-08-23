using Ambev.DeveloperEvaluation.Domain.Factories;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemQuantity { get; set; }
        public decimal UnitProductItemPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalSaleItemPrice { get; set; }
        public decimal TotalWithoutDiscount { get; set; }
        public string SaleItemStatus { get; set; }
        public SaleItem()
        {
            TotalSaleItemPrice = 0;
        }

        public decimal CalculateTotalSaleItemPrice()
        {
            TotalWithoutDiscount = UnitProductItemPrice * ProductItemQuantity;
            DiscountAmount = DiscountAmountCalculatorFactory.Create(ProductItemQuantity).Calculate(TotalWithoutDiscount);
            TotalSaleItemPrice = TotalWithoutDiscount - DiscountAmount;
            return TotalSaleItemPrice;
        }
    }
}
