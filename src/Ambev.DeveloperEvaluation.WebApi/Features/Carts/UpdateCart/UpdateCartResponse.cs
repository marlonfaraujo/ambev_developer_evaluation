using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart
{
    public record UpdateCartResponse(Guid UserId, Guid BranchSaleId, decimal TotalSalePrice, IEnumerable<SaleItem> SaleItems)
    {
    }
}
