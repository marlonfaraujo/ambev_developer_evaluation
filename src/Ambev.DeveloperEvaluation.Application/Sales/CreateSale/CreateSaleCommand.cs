using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequestApplication<CreateSaleResult>
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public Guid BranchSaleId { get; set; }
    public decimal TotalSalePrice { get; set; }
    public IEnumerable<SaleItem> SaleItems { get; set; }
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