namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart
{
    public record CreateCartItemsRequest(Guid ProductId, int ProductItemQuantity)
    {
    }
}
