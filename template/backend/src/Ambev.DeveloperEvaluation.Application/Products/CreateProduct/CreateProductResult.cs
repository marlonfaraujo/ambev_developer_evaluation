namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public record CreateProductResult(Guid Id, string Name, string Description, decimal Price)
    {
    }
}
