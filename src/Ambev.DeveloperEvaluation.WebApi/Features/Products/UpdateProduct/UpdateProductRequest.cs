namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    public record UpdateProductRequest (Guid Id, string Name, string Description, decimal Price)
    {
    }
}
