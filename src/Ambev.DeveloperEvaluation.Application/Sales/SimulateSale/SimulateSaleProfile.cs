using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleProfile : Profile
    {
        public SimulateSaleProfile()
        {
            CreateMap<SimulateSaleQuery, Sale>();
            CreateMap<Sale, SimulateSaleResult>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BranchSaleId, opt => opt.MapFrom(src => src.BranchSaleId))
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => src.TotalSalePrice.Value))
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));
        }
    }
}
