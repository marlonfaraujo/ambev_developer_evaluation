using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartCommand : IRequestApplication<CreateCartResult>
    {
        public Guid UserId { get; set; }
        public Guid BranchSaleId { get; set; }
        public decimal TotalSalePrice { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
    }
}
