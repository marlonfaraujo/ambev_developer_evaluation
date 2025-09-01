using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductItemQuantity { get; set; }
        public Money UnitProductItemPrice { get; set; }
        public Money DiscountAmount { get; private set; }
        public Money TotalSaleItemPrice { get; private set; }
        public Money TotalWithoutDiscount { get; private set; }

        public CartItem(Guid productId, string productName, int productItemQuantity, Money unitProductItemPrice, Money discountAmount, Money totalSaleItemPrice, Money totalWithoutDiscount)
        {
            ProductId = productId;
            ProductName = productName;
            ProductItemQuantity = productItemQuantity;
            UnitProductItemPrice = unitProductItemPrice;
            DiscountAmount = discountAmount;
            TotalSaleItemPrice = totalSaleItemPrice;
            TotalWithoutDiscount = totalWithoutDiscount;
        }

        public CartItem()
        {
            UnitProductItemPrice = new Money(0);
            DiscountAmount = new Money(0);
            TotalSaleItemPrice = new Money(0);
            TotalWithoutDiscount = new Money(0);
        }
    }
}
