using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public record GetCartResult(Guid Id, Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<GetCartItemResult> CartItems)
    {
    }
}
