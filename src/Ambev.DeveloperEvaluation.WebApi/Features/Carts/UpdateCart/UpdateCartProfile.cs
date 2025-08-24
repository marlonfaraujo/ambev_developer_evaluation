using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart
{
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<UpdateCartRequest, SimulateSaleQuery>();
            CreateMap<SimulateSaleResult, UpdateCartResponse>();
        }
    }
}
