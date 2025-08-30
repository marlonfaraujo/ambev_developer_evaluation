using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public record CreateCartResult(Guid Id, Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<CartItem> CartItems)
    {
    }
}
