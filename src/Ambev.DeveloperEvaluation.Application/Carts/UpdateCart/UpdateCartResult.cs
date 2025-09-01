using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public record UpdateCartResult(Guid Id, Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<CartItem> CartItems)
    {
    }
}
