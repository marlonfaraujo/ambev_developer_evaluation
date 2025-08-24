using Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.DeleteBranch
{
    public class DeteleBranchProfile : Profile
    {
        public DeteleBranchProfile()
        {
            CreateMap<DeleteBranchRequest, DeleteBranchCommand>();
        }
    }
}
