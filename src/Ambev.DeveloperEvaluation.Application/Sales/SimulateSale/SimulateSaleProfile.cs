using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleProfile : Profile
    {
        public SimulateSaleProfile()
        {
            CreateMap<SimulateSaleQuery, Sale>();
            CreateMap<Sale, SimulateSaleResult>();
        }
    }
}
