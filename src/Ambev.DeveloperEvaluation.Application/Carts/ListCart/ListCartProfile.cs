using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartProfile : Profile
    {
        public ListCartProfile()
        {
            CreateMap<Cart, ListCartResultItem>()
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => src.TotalSalePrice.Value));
        }
    }
}
