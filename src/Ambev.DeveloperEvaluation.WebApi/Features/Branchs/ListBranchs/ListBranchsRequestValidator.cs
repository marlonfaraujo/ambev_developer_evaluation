using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs
{
    public class ListBranchsRequestValidator : AbstractValidator<ListBranchsRequest>
    {
        public ListBranchsRequestValidator()
        {
            /*
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
            */
        }
    }
}
