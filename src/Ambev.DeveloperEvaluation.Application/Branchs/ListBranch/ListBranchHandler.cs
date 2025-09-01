using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branchs.ListBranch
{
    public class ListBranchHandler : IRequestApplicationHandler<ListBranchQuery, ListBranchResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public ListBranchHandler(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<ListBranchResult> Handle(ListBranchQuery request, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object>();
            var options = new QueryOptions();

            if (!string.IsNullOrWhiteSpace(request.Name))
                filters["Name"] = request.Name;

            if (request.PageNumber > 0)
            {
                options.Page = request.PageNumber;
            }
            if (request.PageSize > 0)
            {
                options.PageSize = request.PageSize;
            }
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                options.SortBy = request.SortBy;
                options.SortDescending = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
            }
            options.Filters = filters;

            var pagedResult = await _branchRepository.GetPagedAsync(options, cancellationToken);
            var items = _mapper.Map<IEnumerable<ListBranchResultData>>(pagedResult.Items);
            return new ListBranchResult
            {
                Items = items,
                TotalCount = pagedResult.TotalCount,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
        }
    }
}
