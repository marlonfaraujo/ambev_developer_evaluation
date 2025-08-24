using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public record GetBranchCommand(Guid Id, string Name, string Description) : IRequest<GetBranchResult>
    {
        public ValidationResultDetail Validate()
        {
            var validator = new GetBranchCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
