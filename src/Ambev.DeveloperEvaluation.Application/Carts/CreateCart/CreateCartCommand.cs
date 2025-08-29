using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public record CreateCartCommand(Guid UserId, Guid BranchSaleId, decimal TotalSalePrice) : IRequestApplication<CreateCartResult>
    {
    }
}
