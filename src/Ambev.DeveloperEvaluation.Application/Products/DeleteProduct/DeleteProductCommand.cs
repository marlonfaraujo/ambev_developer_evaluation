using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : IRequestApplication<DeleteProductResult>
    {
    }
}
