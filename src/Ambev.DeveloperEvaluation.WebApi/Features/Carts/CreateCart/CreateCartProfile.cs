using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartRequest, SimulateSaleQuery>();
            CreateMap<CreateCartItemsRequest, CartItem>();
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
