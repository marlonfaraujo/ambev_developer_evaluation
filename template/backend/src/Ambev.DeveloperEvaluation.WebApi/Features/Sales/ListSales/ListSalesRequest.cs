namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public record ListSalesRequest(
        Guid? Id = null,
        string? BranchName = null,
        string? ProductName = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        int PageNumber = 1,
        int PageSize = 10
    )
    {
    }
}
