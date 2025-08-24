namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs
{
    public record ListBranchsRequest(string? Name, int PageNumber = 1, int PageSize = 10)
    {
    }
}
