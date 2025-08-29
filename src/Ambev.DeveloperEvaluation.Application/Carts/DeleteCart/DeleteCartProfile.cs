using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartProfile : Profile
    {
        public DeleteCartProfile()
        {
            CreateMap<Cart, DeleteCartResult>();
        }
    }
}
