using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductCommandValidator : AbstractValidator<GetProductCommand>
    {
        public GetProductCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("Product ID is required"); ;
        }
    }
}
