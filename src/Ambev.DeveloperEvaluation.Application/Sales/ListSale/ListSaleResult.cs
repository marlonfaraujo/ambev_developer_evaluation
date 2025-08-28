namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleResultItem
    {
        public Guid SaleId { get; set; }
        public int SaleNumber { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal TotalSalePrice { get; set; }
        public string SaleStatus { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchDescription { get; set; } = string.Empty;
        public IEnumerable<ListSaleItemResult> SaleItems { get; set; }
    }

    public class ListSaleResult
    {
        public IEnumerable<ListSaleResultItem> Items { get; set; } = new List<ListSaleResultItem>();
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ListSaleItemResult
    {
        public Guid SaleItemId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductItemQuantity { get; set; }
        public decimal UnitProductItemPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalSaleItemPrice { get; set; }
        public decimal TotalWithoutDiscount { get; set; }
        public string SaleItemStatus { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }
}
