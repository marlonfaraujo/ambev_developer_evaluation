using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<Cart, UpdateCartResult>()
                .ForMember(dest => dest.TotalSalePrice, opt => opt.MapFrom(src => src.TotalSalePrice.Value));
        }
    }
}
