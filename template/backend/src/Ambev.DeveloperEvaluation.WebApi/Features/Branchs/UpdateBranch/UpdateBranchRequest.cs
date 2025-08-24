namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.UpdateBranch
{
    public record UpdateBranchRequest(Guid Id, string Name, string Description)
    {
    }
}
