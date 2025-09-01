namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public record UpdateSaleResult(Guid Id, int SaleNumber, DateTime SaleDate, Guid UserId, decimal TotalSalePrice, Guid BranchSaleId, string SaleStatus, IEnumerable<UpdateSaleItemResult> SaleItems)
    {
    }

    public record UpdateSaleItemResult(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}
