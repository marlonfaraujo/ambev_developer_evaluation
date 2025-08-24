using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch
{
    public class GetBranchRequestValidator : AbstractValidator<GetBranchRequest>
    {
        public GetBranchRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

        }
    }
}
