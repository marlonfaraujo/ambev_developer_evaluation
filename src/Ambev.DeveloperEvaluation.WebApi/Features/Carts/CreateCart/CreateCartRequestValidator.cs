using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
    {
        public CreateCartRequestValidator()
        {
            RuleFor(x => x.BranchSaleId)
                .NotEmpty().WithMessage("BranchId ID is required.")
                .NotNull().WithMessage("BranchId ID cannot be null.");

            RuleFor(x => x.CartItems)
                .NotEmpty().WithMessage("Items is required.");
        }
    }
}
