using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchProfile : Profile
    {
        public UpdateBranchProfile()
        {
            CreateMap<UpdateBranchCommand, Branch>();
            CreateMap<Branch, UpdateBranchResult>();
        }
    }
}
