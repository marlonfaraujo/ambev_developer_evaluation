using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public record GetBranchQuery(Guid Id) : IRequestApplication<GetBranchResult>
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
