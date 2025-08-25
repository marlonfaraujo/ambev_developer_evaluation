using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch
{
    public record CreateBranchCommand(string Name, string Description) : IRequestApplication<CreateBranchResult>
    {
    }
}
