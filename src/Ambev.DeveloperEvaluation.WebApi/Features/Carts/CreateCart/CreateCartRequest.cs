namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart
{
    public record CreateCartRequest(Guid BranchSaleId, IEnumerable<CreateCartItemsRequest> SaleItems)
    {
    }
}
