using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartProfile : Profile
    {
        public GetCartProfile()
        {
            CreateMap<Cart, GetCartResult>()
                .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
                .ForCtorParam("UserId", opt => opt.MapFrom(x => x.UserId))
                .ForCtorParam("BranchSaleId", opt => opt.MapFrom(x => x.BranchSaleId))
                .ForCtorParam("BranchName", opt => opt.MapFrom(x => x.BranchName))
                .ForCtorParam("TotalSalePrice", opt => opt.MapFrom(x => (decimal)x.TotalSalePrice.Value))
                .ForCtorParam("CartItems", opt => opt.MapFrom(x => x.CartItems));

            CreateMap<CartItem, GetCartItemResult>()
                .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
                .ForCtorParam("ProductId", opt => opt.MapFrom(x => x.ProductId))
                .ForCtorParam("ProductName", opt => opt.MapFrom(x => x.ProductName))
                .ForCtorParam("ProductItemQuantity", opt => opt.MapFrom(x => x.ProductItemQuantity))
                .ForCtorParam("UnitProductItemPrice", opt => opt.MapFrom(x => (decimal)x.UnitProductItemPrice.Value))
                .ForCtorParam("DiscountAmount", opt => opt.MapFrom(x => (decimal)x.DiscountAmount.Value))
                .ForCtorParam("TotalSaleItemPrice", opt => opt.MapFrom(x => (decimal)x.TotalSaleItemPrice.Value))
                .ForCtorParam("TotalWithoutDiscount", opt => opt.MapFrom(x => (decimal)x.TotalWithoutDiscount.Value));
        }
    }
}
