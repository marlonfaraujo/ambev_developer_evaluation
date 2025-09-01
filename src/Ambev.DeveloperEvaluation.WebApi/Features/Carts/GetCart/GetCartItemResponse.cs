namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    public record GetCartItemResponse(Guid Id, Guid ProductId, string ProductName, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount)
    {
    }
}
