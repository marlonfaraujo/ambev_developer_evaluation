namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public record ListSalesRequest(
        Guid? SaleId = null,
        Guid? UserId = null,
        Guid? BranchId = null,
        int PageNumber = 1,
        int PageSize = 10,
        string SortBy = "",
        string SortDirection = ""
    )
    {
    }
}
