using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public record UpdateCartCommand(Guid Id, Guid UserId, Guid BranchSaleId, decimal TotalSalePrice) : IRequestApplication<UpdateCartResult>
    {
    }
}
