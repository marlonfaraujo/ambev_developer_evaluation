using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchQueryValidator : AbstractValidator<GetBranchQuery>
    {
        public GetBranchQueryValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("Branch ID is required"); ;
        }
    }
}
