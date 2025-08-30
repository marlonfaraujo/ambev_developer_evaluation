using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public Money TotalSalePrice { get; set; }
        public Guid BranchSaleId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public ICollection<CartItem> CartItems { get; private set; }


        public Cart(Guid userId, Money totalSalePrice, Guid branchSaleId, string branchName)
        {
            UserId = userId;
            TotalSalePrice = totalSalePrice;
            BranchSaleId = branchSaleId;
            BranchName = branchName;
            CartItems = new List<CartItem>();
        }

        public Cart(Guid userId, Money totalSalePrice, Guid branchSaleId, string branchName, ICollection<CartItem> cartItems)
        {
            UserId = userId;
            TotalSalePrice = totalSalePrice;
            BranchSaleId = branchSaleId;
            BranchName = branchName;
            CartItems = cartItems;
        }

        public Cart()
        {
        }
    }
}
