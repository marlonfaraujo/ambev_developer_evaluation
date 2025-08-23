namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int ItemQuantity { get; set; }
        public decimal UnitItemPrice { get; set; }
        public decimal ItemDiscount { get; set; }
        public decimal TotalItemPrice { get; set; }
        public string SaleItemStatus { get; set; }
        public SaleItem()
        {
            ItemDiscount = 0;
            TotalItemPrice = 0;
        }
    }
}
