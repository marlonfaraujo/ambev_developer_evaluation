using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductQuery : IRequestApplication<ListProductResult>
    {
        public string? Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }
}
