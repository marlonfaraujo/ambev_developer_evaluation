using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price) : IRequest<CreateProductResult> 
    {
    }
}
