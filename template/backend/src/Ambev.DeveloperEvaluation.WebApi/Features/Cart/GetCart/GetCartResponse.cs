using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart
{
    public record GetCartResponse(decimal TotalSalePrice, IEnumerable<SaleItem> SaleItems)
    {
    }
}
