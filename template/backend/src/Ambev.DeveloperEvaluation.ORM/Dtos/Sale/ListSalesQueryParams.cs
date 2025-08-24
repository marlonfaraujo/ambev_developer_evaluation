namespace Ambev.DeveloperEvaluation.ORM.Dtos.Sale
{
    public class ListSalesQueryParams
    {
        public ListSalesQueryParams()
        {
            Pager = new Pager();
        }

        public string SaleId { get; set; } = string.Empty;
        public int SaleNumber { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public Pager Pager { get; set; }
    }
}
