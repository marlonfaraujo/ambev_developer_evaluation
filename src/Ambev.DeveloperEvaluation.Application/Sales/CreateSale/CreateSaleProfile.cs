using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(src => src.BranchSaleId))
            .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => new Money(src.TotalSalePrice)))
            .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));

        CreateMap<Sale, CreateSaleResult>()
            .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
            .ForCtorParam("UserId", opt => opt.MapFrom(x => x.UserId))
            .ForCtorParam("BranchSaleId", opt => opt.MapFrom(x => x.BranchSaleId))
            .ForCtorParam("SaleNumber", opt => opt.MapFrom(x => x.SaleNumber))
            .ForCtorParam("TotalSalePrice", opt => opt.MapFrom(x => (decimal)x.TotalSalePrice.Value))
            .ForCtorParam("SaleStatus", opt => opt.MapFrom(x => x.SaleStatus))
            .ForCtorParam("SaleItems", opt => opt.MapFrom(x => x.SaleItems));

        CreateMap<SaleItem, CreateSaleItemResult>()
            .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
            .ForCtorParam("ProductId", opt => opt.MapFrom(x => x.ProductId))
            .ForCtorParam("ProductItemQuantity", opt => opt.MapFrom(x => x.ProductItemQuantity))
            .ForCtorParam("UnitProductItemPrice", opt => opt.MapFrom(x => (decimal)x.UnitProductItemPrice.Value))
            .ForCtorParam("DiscountAmount", opt => opt.MapFrom(x => (decimal)x.DiscountAmount.Value))
            .ForCtorParam("TotalSaleItemPrice", opt => opt.MapFrom(x => (decimal)x.TotalSaleItemPrice.Value))
            .ForCtorParam("TotalWithoutDiscount", opt => opt.MapFrom(x => (decimal)x.TotalWithoutDiscount.Value));

        CreateMap<CartItem, CreateSaleItem>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(x => x.ProductId))
            .ForMember(dest => dest.ProductItemQuantity, opt => opt.MapFrom(x => x.ProductItemQuantity))
            .ForMember(dest => dest.UnitProductItemPrice, opt => opt.MapFrom(x => (decimal)x.UnitProductItemPrice.Value))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(x => (decimal)x.DiscountAmount.Value))
            .ForMember(dest => dest.TotalSaleItemPrice, opt => opt.MapFrom(x => (decimal)x.TotalSaleItemPrice.Value))
            .ForMember(dest => dest.TotalWithoutDiscount, opt => opt.MapFrom(x => (decimal)x.TotalWithoutDiscount.Value));

        CreateMap<CreateSaleItem, SaleItem>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductItemQuantity, opt => opt.MapFrom(src => src.ProductItemQuantity))
            .ForMember(dest => dest.UnitProductItemPrice, opt => opt.MapFrom(src => new Money(src.UnitProductItemPrice)))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => new Money(src.DiscountAmount)))
            .ForMember(dest => dest.TotalSaleItemPrice, opt => opt.MapFrom(src => new Money(src.TotalSaleItemPrice)))
            .ForMember(dest => dest.TotalWithoutDiscount, opt => opt.MapFrom(src => new Money(src.TotalWithoutDiscount)));

        CreateMap<Cart, CreateSaleCommand>()
            .ForMember(dest => dest.UserId, (opt => opt.MapFrom(x => x.UserId)))
            .ForMember(dest => dest.TotalSalePrice, (opt => opt.MapFrom(x => x.TotalSalePrice.Value)))
            .ForMember(dest => dest.BranchSaleId, (opt => opt.MapFrom(x => x.BranchSaleId)))
            .ForMember(dest => dest.SaleItems, (opt => opt.MapFrom(x => x.CartItems)));


        CreateMap<Cart, Sale>()
            .ForMember(dest => dest.UserId, (opt => opt.MapFrom(x => x.UserId)))
            .ForMember(dest => dest.TotalSalePrice, (opt => opt.MapFrom(x => x.TotalSalePrice)))
            .ForMember(dest => dest.BranchSaleId, (opt => opt.MapFrom(x => x.BranchSaleId)))
            .ForMember(dest => dest.SaleItems, (opt => opt.MapFrom(x => x.CartItems)));
    }
}
