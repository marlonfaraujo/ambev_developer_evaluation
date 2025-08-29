using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartProfile : Profile
    {
        public ListCartProfile()
        {
            CreateMap<Cart, ListCartResultData>(); ;
            CreateMap<CartItem, ListCartItemResult>();
        }
    }
}
