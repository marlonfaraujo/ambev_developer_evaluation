using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleCommandValidator : AbstractValidator<SimulateSaleCommand>
    {
        public SimulateSaleCommandValidator()
        {
            RuleFor(sale => sale.BranchSaleId).NotEmpty();

            RuleFor(sale => sale.SaleItems).NotEmpty();
        }
    }
}
