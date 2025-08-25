using Ambev.DeveloperEvaluation.Application.Requests;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public record DeleteSaleCommand(Guid Id) : IRequestApplication<DeleteSaleResult>
{
}
