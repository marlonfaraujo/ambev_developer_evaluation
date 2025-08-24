using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart
{
    public class GetCartProfile : Profile
    {
        public GetCartProfile()
        {
            CreateMap<CreateSaleResult, GetCartResponse>();
        }
    }
}
