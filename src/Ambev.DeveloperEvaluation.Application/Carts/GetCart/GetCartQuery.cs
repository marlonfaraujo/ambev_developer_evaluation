using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public record GetCartQuery(Guid Id) : IRequestApplication<GetCartResult>
    {
    }
}
