using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public record CreateCartResult(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice, IEnumerable<CartItem> CartItems)
    {
    }
}
