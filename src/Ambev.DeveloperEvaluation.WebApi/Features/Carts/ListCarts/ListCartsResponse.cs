namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts
{
    public class ListCartsResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BranchSaleId { get; set; }
        public decimal TotalSalePrice { get; set; }
        public string BranchName { get; set; }
        public IEnumerable<ListCartItemResponse> CartItems { get; set; }
    }


    public class ListCartItemResponse
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
