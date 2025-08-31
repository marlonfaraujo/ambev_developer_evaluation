namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public record CreateCartRequest(Guid BranchSaleId, IEnumerable<CreateCartItemsRequest> CartItems)
    {
    }
}
