namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public record UpdateSaleRequest(
        Guid Id,
        Guid BranchSaleId,
        string SaleStatus,
        IEnumerable<UpdateSaleItemRequest> SaleItems
    )
    {
    }
}
