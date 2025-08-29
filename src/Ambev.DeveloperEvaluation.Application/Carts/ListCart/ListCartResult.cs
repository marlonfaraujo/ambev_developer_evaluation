namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartResult
    {
        public IEnumerable<ListCartResultItem> Items { get; set; }
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ListCartResultItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BranchSaleId { get; set; }
        public decimal TotalSalePrice { get; set; }
        public string BranchName { get; set; }
    }
}
