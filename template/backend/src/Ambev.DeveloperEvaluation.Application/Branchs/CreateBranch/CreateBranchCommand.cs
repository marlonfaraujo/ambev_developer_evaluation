using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch
{
    public record CreateBranchCommand(string Name, string Description) : IRequest<CreateBranchResult>
    {
    }
}
