using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesProfile : Profile
    {
        public ListSalesProfile()
        {
            CreateMap<ListSaleResultItem, ListSalesResponse>();
            CreateMap<ListSaleItemResult, ListSaleItemResponse>();
            CreateMap<ListSalesRequest, ListSaleQuery>();
        }
    }
}
