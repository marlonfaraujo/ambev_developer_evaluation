namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public record ListProductsRequest(
        string? Name,
        int PageNumber = 1,
        int PageSize = 10,
        string SortBy = "",
        string SortDirection = "")
    {
    }
}
