using Ambev.DeveloperEvaluation.Application.Branchs.ListBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs
{
    public class ListBranchsProfile : Profile
    {
        public ListBranchsProfile()
        {
            CreateMap<ListBranchResultData, ListBranchsResponse>();
            CreateMap<ListBranchsRequest, ListBranchQuery>();
        }
    }
}
