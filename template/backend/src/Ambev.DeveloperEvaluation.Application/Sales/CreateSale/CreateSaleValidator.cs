using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.UserId).NotEmpty();
        RuleFor(sale => sale.SaleItems).NotEmpty();
        RuleFor(sale => sale.BranchSaleId).NotEmpty();
    }
}