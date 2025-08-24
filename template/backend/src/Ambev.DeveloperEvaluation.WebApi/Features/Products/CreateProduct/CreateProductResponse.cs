namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    public record CreateProductResponse(Guid Id, string Name, string Description, decimal Price)
    {
    }
}
