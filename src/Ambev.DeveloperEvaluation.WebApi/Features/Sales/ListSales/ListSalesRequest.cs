namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public record ListSalesRequest(
        string SaleId = null,
        string UserId= null,
        string BranchId = null,
        string ProductId = null,
        int PageNumber = 1,
        int PageSize = 10
    )
    {
    }
}
