using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateCartResponse>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.BranchSaleId)
                .NotEmpty().WithMessage("BranchId ID is required.")
                .NotNull().WithMessage("BranchId ID cannot be null.");

            RuleFor(x => x.SaleItems)
                .NotEmpty().WithMessage("Items is required.");
        }
    }
}
