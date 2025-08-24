using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleResult(Guid Id, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<SaleItem> SaleItems)
{
}
