namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public record ListProductsResponse(Guid Id, string Name, string Description, decimal Price)
    {
    }
}
