using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public record CreateSaleRequest(Guid UserId, Guid BranchSaleId, IEnumerable<SaleItem> SaleItems)
    {
    }
}
