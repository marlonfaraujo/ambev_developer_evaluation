using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart
{
    public record UpdateCartResponse(Guid UserId, Guid BranchId, decimal TotalSalePrice, IEnumerable<SaleItem> SaleItems)
    {
    }
}
