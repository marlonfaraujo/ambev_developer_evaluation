using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<Cart, UpdateCartResult>()
                .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
                .ForCtorParam("UserId", opt => opt.MapFrom(x => x.UserId))
                .ForCtorParam("BranchSaleId", opt => opt.MapFrom(x => x.BranchSaleId))
                .ForCtorParam("BranchName", opt => opt.MapFrom(x => x.BranchName))
                .ForCtorParam("TotalSalePrice", opt => opt.MapFrom(x => (decimal)x.TotalSalePrice.Value))
                .ForCtorParam("CartItems", opt => opt.MapFrom(x => x.CartItems));
            CreateMap<UpdateCartCommand, Cart>();
        }
    }
}
