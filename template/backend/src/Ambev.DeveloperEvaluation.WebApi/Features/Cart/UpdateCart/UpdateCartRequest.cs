using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart
{
    public record UpdateCartRequest(Guid BranchSaleId, IEnumerable<CreateCartItemsRequest> SaleItems)
    {
    }
}
