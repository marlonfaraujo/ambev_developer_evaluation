namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public record UpdateProductResult(Guid Id, string Name, string Description, decimal Price)
    {
    }
}
