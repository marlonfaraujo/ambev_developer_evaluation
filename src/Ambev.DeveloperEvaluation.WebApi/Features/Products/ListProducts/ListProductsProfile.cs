using Ambev.DeveloperEvaluation.ORM.Dtos.Product;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsProfile : Profile
    {
        public ListProductsProfile()
        {
            CreateMap<ListProductsQueryResult, ListProductsResponse>();
        }
    }
}
