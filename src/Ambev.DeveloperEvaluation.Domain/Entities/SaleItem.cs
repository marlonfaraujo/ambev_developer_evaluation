using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Factories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public int ProductItemQuantity { get; set; }
        public Money UnitProductItemPrice { get; set; }
        public Money DiscountAmount { get; private set; }
        public Money TotalSaleItemPrice { get; private set; }
        public Money TotalWithoutDiscount { get; private set; }
        public string SaleItemStatus { get; private set; }
        public SaleItem()
        {
            TotalSaleItemPrice = new Money(0);
            SaleItemStatus = SaleStatusEnum.Created.ToString();
        }

        public SaleItem(Guid id, Guid productId, int productItemQuantity, decimal unitProductItemPrice, decimal discountAmount, decimal totalSaleItemPrice, decimal totalWithoutDiscount, string saleItemStatus)
        {
            Id = id;
            ProductId = productId;
            ProductItemQuantity = productItemQuantity;
            UnitProductItemPrice = new Money(unitProductItemPrice);
            DiscountAmount = new Money(discountAmount);
            TotalSaleItemPrice = new Money(totalSaleItemPrice);
            TotalWithoutDiscount = new Money(totalWithoutDiscount);
            SaleItemStatus = saleItemStatus;
        }

        public decimal CalculateTotalSaleItemPrice()
        {
            TotalWithoutDiscount = new Money(UnitProductItemPrice.Value * ProductItemQuantity);
            DiscountAmount = new Money(DiscountAmountCalculatorFactory.Create(ProductItemQuantity).Calculate(TotalWithoutDiscount.Value));
            TotalSaleItemPrice = new Money(TotalWithoutDiscount.Value - DiscountAmount.Value);
            return TotalSaleItemPrice.Value;
        }

        public void SetItemQuantity(int quantity)
        {
            ProductItemQuantity = quantity;
        }

        public SaleItemCancelledEvent CancelItem()
        {
            SaleItemStatus = SaleItemStatusEnum.Cancelled.ToString();
            return new SaleItemCancelledEvent(this.Id);
        }
    }
}
