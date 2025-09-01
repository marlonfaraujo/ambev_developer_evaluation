using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branchs.ListBranch
{
    public class ListBranchProfile : Profile
    {
        public ListBranchProfile()
        {
            CreateMap<Branch, ListBranchResultData>();
        }
    }
}
