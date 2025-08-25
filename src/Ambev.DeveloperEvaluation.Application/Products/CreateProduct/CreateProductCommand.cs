using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price) : IRequestApplication<CreateProductResult> 
    {
    }
}
