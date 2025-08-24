using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsProfile : Profile
    {
        public ListProductsProfile()
        {
            CreateMap<ListProductsRequest, GetProductCommand>();
            CreateMap<GetProductResult, ListProductsResponse>();
        }
    }
}
