using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<GetSaleRequest, GetSaleQuery>();
            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<GetSaleItemResult, GetSaleItemResponse>();

            CreateMap<SaleItem, CreateSaleItemResult>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductItemQuantity, opt => opt.MapFrom(src => src.ProductItemQuantity))
                .ForMember(dest => dest.UnitProductItemPrice, opt => opt.MapFrom(src => src.UnitProductItemPrice.Value))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount.Value))
                .ForMember(dest => dest.TotalSaleItemPrice, opt => opt.MapFrom(src => src.TotalSaleItemPrice.Value))
                .ForMember(dest => dest.TotalWithoutDiscount, opt => opt.MapFrom(src => src.TotalWithoutDiscount.Value));
        }
    }
}
