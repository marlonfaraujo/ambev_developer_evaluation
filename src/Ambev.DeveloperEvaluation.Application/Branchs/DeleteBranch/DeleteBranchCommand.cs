using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public record DeleteBranchCommand(Guid Id) : IRequestApplication<DeleteBranchResult>
    {
    }
}
