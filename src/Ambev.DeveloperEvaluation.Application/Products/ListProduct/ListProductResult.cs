namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductResult
    {
        public IEnumerable<ListProductResultData> Items { get; set; }
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public class ListProductResultData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ListProductResultData()
        {

        }
    }
}
