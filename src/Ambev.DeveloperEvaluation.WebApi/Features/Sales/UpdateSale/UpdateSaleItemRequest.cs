namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public record UpdateSaleItemRequest(Guid Id, Guid ProductId, int ProductItemQuantity)
    {
    }
}
