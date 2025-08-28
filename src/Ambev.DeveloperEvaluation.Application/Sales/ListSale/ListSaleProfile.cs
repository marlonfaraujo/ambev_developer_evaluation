using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleProfile : Profile
    {
        public ListSaleProfile()
        {
            CreateMap<ISaleModel, ListSaleResultItem>()
                .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.SaleId))
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => src.TotalSalePrice))
                .ForMember(dest => dest.SaleStatus, opt => opt.MapFrom(src => src.SaleStatus))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.BranchDescription, opt => opt.MapFrom(src => src.BranchDescription))
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));


            CreateMap<ISaleItemModel, ListSaleItemResult>();
        }
    }
}
