namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public record CreateCartResponse(Guid Id, Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<CreateCartItemResponse> CartItems, string CartStatus)
    {
    }

    public record CreateCartItemResponse(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount)
    {
    }
}
