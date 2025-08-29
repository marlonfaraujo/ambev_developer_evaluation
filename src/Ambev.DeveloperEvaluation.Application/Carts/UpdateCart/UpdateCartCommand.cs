using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public record UpdateCartCommand(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice, IEnumerable<CartItem> CartItems) : IRequestApplication<UpdateCartResult>
    {
    }
}
