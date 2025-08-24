using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CheckoutCart
{
    public class CheckoutCartValidator : AbstractValidator<CreateCartResponse>
    {
        public CheckoutCartValidator()
        {
            RuleFor(x => x.BranchSaleId)
                .NotEmpty().WithMessage("BranchId ID is required.")
                .NotNull().WithMessage("BranchId ID cannot be null.");

            RuleFor(x => x.SaleItems)
                .NotEmpty().WithMessage("Items is required.");
        }
    }
}
