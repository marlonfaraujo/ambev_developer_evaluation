namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesResponse
    {
        public ListSalesResponse() { }

        public ListSalesResponse(ListSalesResponse items)
        {
            SaleId = items.SaleId;
            SaleNumber = items.SaleNumber;
            SaleDate = items.SaleDate;
            TotalSalePrice = items.TotalSalePrice;
            SaleStatus = items.SaleStatus;
            UserId = items.UserId;
            UserName = items.UserName;
            BranchId = items.BranchId;
            BranchName = items.BranchName;
            BranchDescription = items.BranchDescription;
        }

        public Guid SaleId { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalSalePrice { get; set; }
        public string SaleStatus { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchDescription { get; set; } = string.Empty;
        public IEnumerable<ListSaleItemResponse> SaleItems { get; set; } = new List<ListSaleItemResponse>();
    }

    public class ListSaleItemResponse
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
