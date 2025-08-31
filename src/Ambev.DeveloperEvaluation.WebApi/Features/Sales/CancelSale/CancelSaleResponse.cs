namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public record CancelSaleResponse(Guid Id, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<CancelSaleItemResponse> SaleItems)
    {
    }
    public record CancelSaleItemResponse(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}
