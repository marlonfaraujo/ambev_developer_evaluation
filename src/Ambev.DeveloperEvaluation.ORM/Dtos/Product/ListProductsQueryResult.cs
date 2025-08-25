namespace Ambev.DeveloperEvaluation.ORM.Dtos.Product
{
    public class ListProductsQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
