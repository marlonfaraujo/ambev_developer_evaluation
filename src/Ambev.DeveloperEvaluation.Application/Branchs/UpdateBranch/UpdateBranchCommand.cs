using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public record UpdateBranchCommand(Guid Id, string Name, string Description) : IRequest<UpdateBranchResult>
    {
        public ValidationResultDetail Validate()
        {
            var validator = new UpdateBranchCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
