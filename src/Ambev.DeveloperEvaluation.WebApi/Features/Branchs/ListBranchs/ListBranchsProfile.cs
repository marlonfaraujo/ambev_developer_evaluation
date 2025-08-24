using Ambev.DeveloperEvaluation.ORM.Dtos.Branch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs
{
    public class ListBranchsProfile : Profile
    {
        public ListBranchsProfile()
        {
            CreateMap<ListBranchsQueryResult, ListBranchsResponse>();
        }
    }
}
