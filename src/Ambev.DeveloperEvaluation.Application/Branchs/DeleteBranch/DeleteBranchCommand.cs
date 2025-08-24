using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public record DeleteBranchCommand(Guid Id) : IRequest<DeleteBranchResult>
    {
    }
}
