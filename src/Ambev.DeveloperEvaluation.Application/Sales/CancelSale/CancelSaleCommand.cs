using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public record CancelSaleCommand(Guid Id) : IRequestApplication<CancelSaleResult>
    {
    }
}
