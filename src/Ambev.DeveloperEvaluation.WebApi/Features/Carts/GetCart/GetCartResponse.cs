using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    public record GetCartResponse(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice, string BranchName, IEnumerable<GetCartItemResponse> CartItems)
    {
    }
}
