using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleQuery : IRequestApplication<SimulateSaleResult>
    {
        public Guid BranchSaleId { get; set; }
        public IEnumerable<SaleItem> SaleItems { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new SimulateSaleQueryValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
