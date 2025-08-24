using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartRequest, SimulateSaleQuery>();
            CreateMap<CreateCartItemsRequest, SaleItem>();
            CreateMap<SimulateSaleResult, CreateCartResponse>();
        }
    }
}
