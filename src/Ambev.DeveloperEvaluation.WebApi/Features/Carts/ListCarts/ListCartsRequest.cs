namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts
{
    public record ListCartsRequest(
        Guid? CartId = null,
        Guid? UserId = null,
        Guid? BranchId = null,
        int PageNumber = 1,
        int PageSize = 10,
        string SortBy = "",
        string SortDirection = ""
    )
    { }
}
