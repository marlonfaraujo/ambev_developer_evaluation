using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs
{
    public class ListBranchsProfile : Profile
    {
        public ListBranchsProfile()
        {
            CreateMap<ListBranchsRequest, GetBranchCommand>();
            CreateMap<GetBranchResult, ListBranchsResponse>();
        }
    }
}
