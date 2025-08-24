using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    internal class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleCommandValidator()
        {
            RuleFor(sale => sale.Id)
                .NotEmpty()
                .WithMessage("Sale ID required");
        }
    }
}
