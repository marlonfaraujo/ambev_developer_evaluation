using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.NoSql
{
    public class SaleModel : ISaleModel
    {
        public Guid Id { get; set; }
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
        public IEnumerable<ISaleItemModel> SaleItems { get; set; } = new List<SaleItemModel>();
    }

    public class SaleItemModel : ISaleItemModel
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

        public SaleItemModel(Guid saleItemId, Guid productId, int productItemQuantity, decimal unitProductItemPrice, decimal discountAmount, decimal totalSaleItemPrice, decimal totalWithoutDiscount, string saleItemStatus, string productName = "", string productDescription = "")
        {
            SaleItemId = saleItemId;
            ProductId = productId;
            ProductItemQuantity = productItemQuantity;
            UnitProductItemPrice = unitProductItemPrice;
            DiscountAmount = discountAmount;
            TotalSaleItemPrice = totalSaleItemPrice;
            TotalWithoutDiscount = totalWithoutDiscount;
            SaleItemStatus = saleItemStatus;
            ProductName = productName;
            ProductDescription = productDescription;
        }
    }
}
