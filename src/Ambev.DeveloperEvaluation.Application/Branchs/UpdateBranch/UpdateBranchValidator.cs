using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
    {
        public UpdateBranchCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name must not exceed 100 characters.");

        }
    }
}
