namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public record GetProductResponse(Guid Id, string Name, string Description, decimal Price)
    {
    }
}
