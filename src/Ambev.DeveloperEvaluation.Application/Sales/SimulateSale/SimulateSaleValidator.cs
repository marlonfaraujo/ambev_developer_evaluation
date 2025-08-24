using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleQueryValidator : AbstractValidator<SimulateSaleQuery>
    {
        public SimulateSaleQueryValidator()
        {
            RuleFor(sale => sale.BranchSaleId).NotEmpty();

            RuleFor(sale => sale.SaleItems).NotEmpty();
        }
    }
}
