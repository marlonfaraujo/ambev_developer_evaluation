using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("Product ID is required"); ;
        }
    }
}
