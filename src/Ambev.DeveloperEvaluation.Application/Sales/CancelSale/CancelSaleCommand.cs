using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public record CancelSaleCommand(Guid Id, string SaleStatus) : IRequest<CancelSaleResult>
    {
    }
}
