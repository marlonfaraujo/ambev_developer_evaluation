namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public record GetCartResult(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice, string BranchName)
    {
    }
}
