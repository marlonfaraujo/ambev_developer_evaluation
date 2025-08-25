using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleResult
    {
        public Guid UserId { get; set; }
        public Guid BranchSaleId { get; set; }
        public decimal TotalSalePrice { get; set; } 
        public IEnumerable<SaleItem> SaleItems { get; set; }
    }
}
