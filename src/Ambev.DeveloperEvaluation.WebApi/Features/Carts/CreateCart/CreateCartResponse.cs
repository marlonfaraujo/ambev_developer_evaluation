using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public record CreateCartResponse(Guid UserId, Guid BranchSaleId, decimal TotalSalePrice, IEnumerable<CartItem> CartItems)
    {
    }
}
