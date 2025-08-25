using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Factories;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public int ProductItemQuantity { get; set; }
        public decimal UnitProductItemPrice { get; set; }
        public decimal DiscountAmount { get; private set; }
        public decimal TotalSaleItemPrice { get; private set; }
        public decimal TotalWithoutDiscount { get; private set; }
        public string SaleItemStatus { get; private set; }
        public SaleItem()
        {
            TotalSaleItemPrice = 0;
            SaleItemStatus = SaleStatusEnum.Created.ToString();
        }

        public SaleItem(Guid id, Guid productId, int productItemQuantity, decimal unitProductItemPrice, decimal discountAmount, decimal totalSaleItemPrice, decimal totalWithoutDiscount, string saleItemStatus)
        {
            Id = id;
            ProductId = productId;
            ProductItemQuantity = productItemQuantity;
            UnitProductItemPrice = unitProductItemPrice;
            DiscountAmount = discountAmount;
            TotalSaleItemPrice = totalSaleItemPrice;
            TotalWithoutDiscount = totalWithoutDiscount;
            SaleItemStatus = saleItemStatus;
        }

        public decimal CalculateTotalSaleItemPrice()
        {
            TotalWithoutDiscount = UnitProductItemPrice * ProductItemQuantity;
            DiscountAmount = DiscountAmountCalculatorFactory.Create(ProductItemQuantity).Calculate(TotalWithoutDiscount);
            TotalSaleItemPrice = TotalWithoutDiscount - DiscountAmount;
            return TotalSaleItemPrice;
        }

        public void SetItemQuantity(int quantity)
        {
            ProductItemQuantity = quantity;
        }

        public SaleItemCancelledEvent CancelItem()
        {
            SaleItemStatus = SaleItemStatusEnum.Cancelled.ToString();
            return new SaleItemCancelledEvent(this);
        }
    }
}
