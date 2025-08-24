using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public record CancelSaleResult(Guid Id, Guid SaleId, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<SaleItem> SaleItems)
    {
    }
}
