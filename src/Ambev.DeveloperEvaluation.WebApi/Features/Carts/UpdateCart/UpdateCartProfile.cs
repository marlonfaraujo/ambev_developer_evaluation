using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<UpdateCartRequest, SimulateSaleQuery>();
            CreateMap<SimulateSaleResult, UpdateCartResponse>();
            CreateMap<SimulateSaleResult, UpdateCartCommand>()
                .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(src => src.BranchSaleId))
                .ForMember(dest => dest.TotalSalePrice , opt => opt.MapFrom(src => src.TotalSalePrice))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.SaleItems));
            CreateMap<UpdateCartResult, UpdateCartResponse>();
        }
    }
}
