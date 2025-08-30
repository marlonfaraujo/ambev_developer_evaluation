namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public record GetCartItemResult(Guid Id, Guid ProductId, string ProductName, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount)
    {
    }
}
