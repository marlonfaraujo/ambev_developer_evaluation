namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public record UpdateSaleResponse(Guid Id, int SaleNumber, DateTime SaleDate, Guid UserId, decimal TotalSalePrice, Guid BranchSaleId, string SaleStatus, IEnumerable<UpdateSaleItemResponse> SaleItems)
    {
    }
    public record UpdateSaleItemResponse(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}
