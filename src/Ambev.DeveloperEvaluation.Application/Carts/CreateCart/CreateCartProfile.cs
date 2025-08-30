using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartCommand, Cart>();
            CreateMap<Cart, CreateCartResult>()
                .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
                .ForCtorParam("UserId", opt => opt.MapFrom(x => x.UserId))
                .ForCtorParam("BranchSaleId", opt => opt.MapFrom(x => x.BranchSaleId))
                .ForCtorParam("BranchName", opt => opt.MapFrom(x => x.BranchName))
                .ForCtorParam("TotalSalePrice", opt => opt.MapFrom(x => (decimal)x.TotalSalePrice.Value))
                .ForCtorParam("CartItems", opt => opt.MapFrom(x => x.CartItems));
        }
    }
}
