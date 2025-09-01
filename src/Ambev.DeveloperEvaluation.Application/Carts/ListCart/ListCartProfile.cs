using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartProfile : Profile
    {
        public ListCartProfile()
        {
            CreateMap<CartItem, ListCartItemResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.ProductItemQuantity, opt => opt.MapFrom(x => x.ProductItemQuantity))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(x => x.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(x => x.ProductName))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(x => x.DiscountAmount.Value))
                .ForMember(dest => dest.UnitProductItemPrice, opt => opt.MapFrom(x => x.UnitProductItemPrice.Value))
                .ForMember(dest => dest.TotalSaleItemPrice, opt => opt.MapFrom(x => x.TotalSaleItemPrice.Value))
                .ForMember(dest => dest.TotalWithoutDiscount, opt => opt.MapFrom(x => x.TotalWithoutDiscount.Value));

            CreateMap<Cart, ListCartResultData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(x => x.BranchSaleId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(x => x.BranchName))
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(x => (decimal)x.TotalSalePrice.Value))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(x => x.CartItems));
        }
    }
}
