using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.NoSql
{
    public class SaleModel
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalSalePrice { get; set; }
        public string SaleStatus { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchDescription { get; set; } = string.Empty;
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
