using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public record CancelSaleResponse(Guid Id, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<SaleItem> SaleItems)
    {
    }
}
