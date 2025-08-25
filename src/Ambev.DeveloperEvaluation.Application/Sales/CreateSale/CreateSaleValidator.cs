using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.UserId).NotEmpty()
                .WithMessage("UserId is required."); ;
        RuleFor(sale => sale.SaleItems).NotEmpty()
                .WithMessage("SaleItems is required."); ;
        RuleFor(sale => sale.BranchSaleId).NotEmpty()
                .WithMessage("BranchSaleId is required."); ;
    }
}