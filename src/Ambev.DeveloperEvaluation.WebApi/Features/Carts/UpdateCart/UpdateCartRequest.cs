using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    public record UpdateCartRequest(Guid BranchSaleId, IEnumerable<CreateCartItemsRequest> CartItems)
    {
    }
}
