using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.ORM.Dtos.Sale
{
    public class ListSalesQueryParams
    {
        public ListSalesQueryParams()
        {
            Pager = new Pager();
        }

        public Guid? SaleId { get; set; } = null;
        public int SaleNumber { get; set; }
        public Guid? UserId { get; set; } = null;
        public Guid? ProductId { get; set; } = null;
        public Guid? BranchId { get; set; } = null;
        public Pager Pager { get; set; }
    }
}
