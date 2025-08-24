using Ambev.DeveloperEvaluation.ORM.Dtos.Sale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesProfile : Profile
    {
        public ListSalesProfile()
        {
            CreateMap<ListSalesQueryResult, ListSalesResponse>();
        }
    }
}
