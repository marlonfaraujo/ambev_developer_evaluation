namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public record CreateCartResult(Guid Id, Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<CreateCartItemResult> CartItems, string CartStatus)
    {
    }
    public record CreateCartItemResult(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount)
    {
    }
}
