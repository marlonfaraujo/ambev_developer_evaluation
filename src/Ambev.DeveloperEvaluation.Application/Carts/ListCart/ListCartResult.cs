namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartResult
    {
        public IEnumerable<ListCartResultData> Items { get; set; }
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ListCartResultData
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BranchSaleId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalSalePrice { get; set; }
        public IEnumerable<ListCartItemResult> CartItems { get; set; }
    }

    public class ListCartItemResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int ProductItemQuantity { get; set; }
        public decimal UnitProductItemPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalSaleItemPrice { get; set; }
        public decimal TotalWithoutDiscount { get; set; }
        public string ProductName { get; set; }
    }
}
