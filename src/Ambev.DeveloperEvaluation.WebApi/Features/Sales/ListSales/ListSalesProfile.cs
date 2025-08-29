using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesProfile : Profile
    {
        public ListSalesProfile()
        {
            CreateMap<ListSaleResultData, ListSalesResponse>();
            CreateMap<ListSaleItemResult, ListSaleItemResponse>();
            CreateMap<ListSalesRequest, ListSaleQuery>();
        }
    }
}
