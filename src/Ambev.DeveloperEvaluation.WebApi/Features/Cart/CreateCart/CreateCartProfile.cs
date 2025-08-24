using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartRequest, SimulateSaleCommand>();
            CreateMap<SimulateSaleResult, CreateCartResponse>();
        }
    }
}
