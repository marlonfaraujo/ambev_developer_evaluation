using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.BranchSaleId).NotEmpty();
            RuleFor(x => x.TotalSalePrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CartItems).NotEmpty();
        }
    }
}
