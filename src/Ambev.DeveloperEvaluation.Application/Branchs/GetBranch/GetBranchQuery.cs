using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public record GetBranchQuery(Guid Id) : IRequest<GetBranchResult>
    {
        public ValidationResultDetail Validate()
        {
            var validator = new GetBranchQueryValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
