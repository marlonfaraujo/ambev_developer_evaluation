using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleQuery : IRequest<SimulateSaleResult>
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
