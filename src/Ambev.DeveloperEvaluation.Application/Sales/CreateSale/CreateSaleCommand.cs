using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequestApplication<CreateSaleResult>
{
    public CreateSaleCommand()
    {
    }

    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public Guid BranchSaleId { get; set; }
    public decimal TotalSalePrice { get; set; }
    public IEnumerable<CreateSaleItem> SaleItems { get; set; }
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

public class CreateSaleItem
{
    public Guid ProductId { get; set; }
    public int ProductItemQuantity { get; set; }
    public decimal UnitProductItemPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalSaleItemPrice { get; set; }
    public decimal TotalWithoutDiscount { get; set; }
}