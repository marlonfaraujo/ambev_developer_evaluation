namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public record GetSaleResponse(Guid Id, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<GetSaleItemResponse> SaleItems)
    {
    }
    public record GetSaleItemResponse(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}
