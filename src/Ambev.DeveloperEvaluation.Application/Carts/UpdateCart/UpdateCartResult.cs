namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public record UpdateCartResult(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice)
    {
    }
}
