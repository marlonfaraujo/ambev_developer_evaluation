using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart
{
    public record CreateCartResponse(Guid BranchSaleId, decimal TotalSalePrice, IEnumerable<SaleItem> SaleItems)
    {
    }
}
