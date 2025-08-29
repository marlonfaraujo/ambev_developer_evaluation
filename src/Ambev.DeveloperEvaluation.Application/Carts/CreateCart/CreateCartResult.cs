namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public record CreateCartResult(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice)
    {
    }
}
