using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public record GetProductQuery(Guid Id, string Name, string Description, decimal Price) : IRequest<GetProductResult>
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
