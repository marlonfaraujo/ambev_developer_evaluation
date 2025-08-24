using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch
{
    public class GetBranchProfile : Profile
    {
        public GetBranchProfile()
        {
            CreateMap<GetBranchRequest, GetBranchQuery>();
            CreateMap<GetBranchResult, GetBranchResponse>();
        }
    }
}
