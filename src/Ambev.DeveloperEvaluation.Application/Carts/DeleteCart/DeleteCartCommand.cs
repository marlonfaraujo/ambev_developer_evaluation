using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public record DeleteCartCommand(Guid Id) : IRequestApplication<DeleteCartResult>
    {
    }
}
