namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public record GetSaleResult(Guid Id, int SaleNumber, decimal TotalSalePrice, string SaleStatus, IEnumerable<GetSaleItemResult> SaleItems)
    {
    }

    public record GetSaleItemResult(Guid Id, Guid ProductId, int ProductItemQuantity, decimal UnitProductItemPrice, decimal DiscountAmount, decimal TotalSaleItemPrice, decimal TotalWithoutDiscount, string SaleItemStatus)
    {
    }
}

