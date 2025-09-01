using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Dtos
{
    public interface ISaleModel
    {
        Guid Id { get; set; }
        Guid SaleId { get; set; }
        int SaleNumber { get; set; }
        DateTime SaleDate { get; set; }
        decimal TotalSalePrice { get; set; }
        string SaleStatus { get; set; }
        Guid UserId { get; set; }
        string UserName { get; set; }
        Guid BranchId { get; set; }
        string BranchName { get; set; }
        string BranchDescription { get; set; }
        IEnumerable<ISaleItemModel> SaleItems { get; set; }
    }

    public interface ISaleItemModel
    {
        Guid SaleItemId { get; set; }
        Guid ProductId { get; set; }
        int ProductItemQuantity { get; set; }
        decimal UnitProductItemPrice { get; set; }
        decimal DiscountAmount { get; set; }
        decimal TotalSaleItemPrice { get; set; }
        decimal TotalWithoutDiscount { get; set; }
        string SaleItemStatus { get; set; }
        string ProductName { get; set; }
        string ProductDescription { get; set; }
    }
}
