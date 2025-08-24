using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchCommandValidator : AbstractValidator<GetBranchCommand>
    {
        public GetBranchCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("Branch ID is required"); ;
        }
    }
}
