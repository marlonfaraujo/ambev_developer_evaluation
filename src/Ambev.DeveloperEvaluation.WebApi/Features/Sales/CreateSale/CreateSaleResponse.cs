namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public record CreateSaleResponse(Guid Id, Guid UserId, Guid BranchSaleId, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<CreateSaleItemResponse> SaleItems)
    {
    }
    public record CreateSaleItemResponse(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}
