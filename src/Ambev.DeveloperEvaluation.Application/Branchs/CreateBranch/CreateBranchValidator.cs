using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch
{
    internal class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("Product name is required.");
        }
    }
}
