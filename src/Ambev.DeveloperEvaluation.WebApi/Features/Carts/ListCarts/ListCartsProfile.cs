using Ambev.DeveloperEvaluation.Application.Carts.ListCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts
{
    public class ListCartsProfile : Profile
    {
        public ListCartsProfile()
        {
            CreateMap<ListCartResultData, ListCartsResponse>();
            CreateMap<ListCartItemResult, ListCartItemResponse>();
            CreateMap<ListCartsRequest, ListCartQuery>();
        }
    }
}
