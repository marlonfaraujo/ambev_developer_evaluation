namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    public record UpdateProductResponse(Guid Id, string Name, string Description, decimal Price)
    {
    }
}
