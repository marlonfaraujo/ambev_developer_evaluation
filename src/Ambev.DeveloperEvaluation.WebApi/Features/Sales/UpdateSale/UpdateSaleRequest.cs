namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public record UpdateSaleRequest(
        Guid Id,
        int SaleNumber,
        Guid UserId,
        decimal TotalSalePrice,
        DateTime SaleDate,
        string SaleStatus
    )
    {
    }
}
