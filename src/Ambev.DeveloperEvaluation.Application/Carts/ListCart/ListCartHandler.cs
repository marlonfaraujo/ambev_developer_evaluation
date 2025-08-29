using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartHandler : IRequestApplicationHandler<ListCartQuery, ListCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public ListCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<ListCartResult> Handle(ListCartQuery request, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object>();
            var options = new QueryOptions();

            if (request.CartId.HasValue)
                filters["CartId"] = request.CartId.Value;
            if (request.UserId.HasValue)
                filters["UserId"] = request.UserId.Value;
            if (request.BranchSaleId.HasValue)
                filters["BranchSaleId"] = request.BranchSaleId.Value;

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

            var pagedResult = await _cartRepository.GetPagedAsync(options, cancellationToken);
            var items = _mapper.Map<IEnumerable<ListCartResultData>>(pagedResult.Items);
            return new ListCartResult
            {
                Items = items,
                TotalCount = pagedResult.TotalCount,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
        }
    }
}
