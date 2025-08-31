namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public record CancelSaleResult(Guid Id, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<CancelSaleItemResult> SaleItems)
    {
    }

    public record CancelSaleItemResult(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}
