using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public record GetProductQuery(Guid Id) : IRequestApplication<GetProductResult>
    {
        public ValidationResultDetail Validate()
        {
            var validator = new GetProductQueryValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
