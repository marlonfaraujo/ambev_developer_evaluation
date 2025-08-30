using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public record CreateCartResponse(Guid Id, Guid UserId, Guid BranchSaleId, string BranchName, decimal TotalSalePrice, IEnumerable<CartItem> CartItems)
    {
    }
}
