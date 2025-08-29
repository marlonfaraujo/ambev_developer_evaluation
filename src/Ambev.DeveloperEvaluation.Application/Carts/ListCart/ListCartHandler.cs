using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
            if (request.UserId.HasValue)
                filters["UserId"] = request.UserId.Value;
            if (request.BranchSaleId.HasValue)
                filters["BranchSaleId"] = request.BranchSaleId.Value;

            var pagedResult = await _cartRepository.GetPagedAsync(
                request.PageNumber > 0 ? request.PageNumber : 1,
                request.PageSize > 0 ? request.PageSize : 10,
                filters,
                request.SortBy,
                string.Equals(request.SortDirection, "desc", System.StringComparison.OrdinalIgnoreCase),
                cancellationToken
            );

            var items = _mapper.Map<IEnumerable<ListCartResultItem>>(pagedResult.Items);
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
