using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public record UpdateSaleResponse(Guid Id, int SaleNumber, DateTime SaleDate, Guid UserId, decimal TotalSalePrice, Guid BranchSaleId, string SaleStatus, IEnumerable<SaleItem> SaleItems)
    {
    }
}
