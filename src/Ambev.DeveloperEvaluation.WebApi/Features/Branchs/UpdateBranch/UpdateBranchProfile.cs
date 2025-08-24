using Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.UpdateBranch
{
    public class UpdateBranchProfile : Profile
    {
        public UpdateBranchProfile()
        {
            CreateMap<UpdateBranchRequest, UpdateBranchCommand>();
            CreateMap<UpdateBranchResult, UpdateBranchResponse>();
        }
    }
}
