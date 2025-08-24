using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public record UpdateSaleResult(Guid Id, Guid SaleId, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<SaleItem> SaleItems)
{
}
