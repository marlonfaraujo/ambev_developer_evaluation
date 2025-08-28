using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Application.Services;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleHandler : IRequestApplicationHandler<ListSaleQuery, ListSaleResult>
    {
        private readonly IMongoDbService<ISaleModel> _mongoDbService;
        private readonly IMapper _mapper;

        public ListSaleHandler(IMongoDbService<ISaleModel> mongoDbService, IMapper mapper)
        {
            _mongoDbService = mongoDbService;
            _mapper = mapper;
        }

        public async Task<ListSaleResult> Handle(ListSaleQuery request, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object>();
            var options = new QueryOptions();

            if (request.SaleId.HasValue)
            {
                filters["SaleId"] = request.SaleId.GetValueOrDefault();
            }
            if (request.BranchId.HasValue)
            {
                filters["BranchId"] = request.BranchId.GetValueOrDefault();
            }
            if (request.ProductId.HasValue)
            {
                filters["ProductId"] = request.ProductId.GetValueOrDefault();
            }
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

            var queryResult = await _mongoDbService.QueryAsync(options);
            var result = new ListSaleResult
            {
                Items = _mapper.Map<IEnumerable<ListSaleResultItem>>(queryResult.Items),
                TotalCount = queryResult.TotalCount,
                Page = queryResult.Page,
                PageSize = queryResult.PageSize
            };
            return result;
        }
    }
}
