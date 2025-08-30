using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>();
        CreateMap<Sale, CreateSaleResult>();

        CreateMap<CartItem, CreateSaleItem>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductItemQuantity, opt => opt.MapFrom(src => src.ProductItemQuantity))
            .ForMember(dest => dest.UnitProductItemPrice, opt => opt.MapFrom(src => src.UnitProductItemPrice.Value))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount.Value))
            .ForMember(dest => dest.TotalSaleItemPrice, opt => opt.MapFrom(src => src.TotalSaleItemPrice.Value))
            .ForMember(dest => dest.TotalWithoutDiscount, opt => opt.MapFrom(src => src.TotalWithoutDiscount.Value));

        CreateMap<CreateSaleItem, SaleItem>();

        CreateMap<Cart, CreateSaleCommand>()
            .ForMember(dest => dest.UserId, (opt => opt.MapFrom(x => x.UserId)))
            .ForMember(dest => dest.TotalSalePrice, (opt => opt.MapFrom(x => x.TotalSalePrice.Value)))
            .ForMember(dest => dest.BranchSaleId, (opt => opt.MapFrom(x => x.BranchSaleId)))
            .ForMember(dest => dest.SaleItems, (opt => opt.MapFrom(x => x.CartItems)));


        CreateMap<Cart, Sale>()
            .ForMember(dest => dest.UserId, (opt => opt.MapFrom(x => x.UserId)))
            .ForMember(dest => dest.TotalSalePrice, (opt => opt.MapFrom(x => x.TotalSalePrice.Value)))
            .ForMember(dest => dest.BranchSaleId, (opt => opt.MapFrom(x => x.BranchSaleId)))
            .ForMember(dest => dest.SaleItems, (opt => opt.MapFrom(x => x.CartItems)));
    }
}
