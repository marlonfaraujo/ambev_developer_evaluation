using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchCommandValidator : AbstractValidator<GetBranchCommand>
    {
        public GetBranchCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .WithMessage("Branch name is required.")
                .MaximumLength(100)
                .WithMessage("Branch name must not exceed 100 characters.");
        }
    }
}
