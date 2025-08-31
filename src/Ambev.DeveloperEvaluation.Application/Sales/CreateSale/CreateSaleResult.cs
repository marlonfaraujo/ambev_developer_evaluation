namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public record CreateSaleResult(Guid Id, Guid UserId, Guid BranchSaleId, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<CreateSaleItemResult> SaleItems)
    {
    }

    public record CreateSaleItemResult(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }

}
