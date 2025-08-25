using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public record UpdateSaleResult(Guid Id, int SaleNumber, DateTime SaleDate, Guid UserId, decimal TotalSalePrice, Guid BranchSaleId, string SaleStatus, IEnumerable<SaleItem> SaleItems)
{
}
