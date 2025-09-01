using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    public record UpdateCartResponse(Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<CartItem> CartItems)
    {
    }
}
