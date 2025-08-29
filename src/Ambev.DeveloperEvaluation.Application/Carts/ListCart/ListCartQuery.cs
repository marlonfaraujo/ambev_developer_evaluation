using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartQuery : IRequestApplication<ListCartResult>
    {
        public Guid? CartId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BranchSaleId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }
}
