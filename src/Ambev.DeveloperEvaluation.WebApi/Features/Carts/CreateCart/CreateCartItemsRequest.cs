namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public record CreateCartItemsRequest(Guid ProductId, int ProductItemQuantity)
    {
    }
}
