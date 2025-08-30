using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartItemsRequest, CartItem>();
            CreateMap<CreateCartItemsRequest, SaleItem>();

            CreateMap<SaleItem, CartItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductItemQuantity, opt => opt.MapFrom(src => src.ProductItemQuantity))
                .ForMember(dest => dest.UnitProductItemPrice, opt => opt.MapFrom(src => new Money(src.UnitProductItemPrice)))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => new Money(src.DiscountAmount)))
                .ForMember(dest => dest.TotalSaleItemPrice, opt => opt.MapFrom(src => new Money(src.TotalSaleItemPrice)))
                .ForMember(dest => dest.TotalWithoutDiscount, opt => opt.MapFrom(src => new Money(src.TotalWithoutDiscount)));

            CreateMap<CreateCartRequest, SimulateSaleQuery>()
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(x => x.CartItems))
                .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(x => x.BranchSaleId));
            CreateMap<SimulateSaleResult, CreateCartResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(src => src.BranchSaleId))
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => src.TotalSalePrice))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.SaleItems));
            CreateMap<SimulateSaleResult, CreateCartCommand>()
                .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(src => src.BranchSaleId))
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => src.TotalSalePrice))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.SaleItems));
            CreateMap<CreateCartResult, CreateCartResponse>(); 
        }
    }
}
