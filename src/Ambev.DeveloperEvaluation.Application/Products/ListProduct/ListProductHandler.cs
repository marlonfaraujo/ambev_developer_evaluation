using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductHandler : IRequestApplicationHandler<ListProductQuery, ListProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICacheDatabase _cacheDatabase;

        public ListProductHandler(IProductRepository productRepository, IMapper mapper, ICacheDatabase cacheDatabase)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheDatabase = cacheDatabase;
        }

        public async Task<ListProductResult> Handle(ListProductQuery request, CancellationToken cancellationToken)
        {
            var queryOptions = GetQueryOptions(request);
            var cachedProducts = await _cacheDatabase.GetAsync<IEnumerable<ListProductResultData>>("products-cache", cancellationToken);
            if (cachedProducts != null && cachedProducts.Any())
            {
                return GetProductsResult(cachedProducts, queryOptions);
            }
            var products = await _productRepository.GetAsync(cancellationToken);
            if (products != null && products.Any())
            {
                var items = _mapper.Map<IEnumerable<ListProductResultData>>(products);
                await _cacheDatabase.SetAsync(
                    "products-cache",
                    items, 
                    cancellationToken, 
                    TimeSpan.FromMinutes(30));

                return GetProductsResult(items, queryOptions);
            }

            return new ListProductResult
            {
                Items = Enumerable.Empty<ListProductResultData>(),
                TotalCount = 0,
                Page = queryOptions.Page,
                PageSize = queryOptions.PageSize
            };
        }

        private ListProductResult GetProductsResult(IEnumerable<ListProductResultData> items, QueryOptions options)
        {
            if (options.Filters != null && options.Filters.Any())
            {
                foreach (var filter in options.Filters)
                {
                    if (filter.Key == "Name" && filter.Value is string nameFilter)
                    {
                        items = items.Where(p => p.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase));
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(options.SortBy))
            {
                var prop = typeof(Product).GetProperty(options.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    if (options.SortDescending)
                        items = items.OrderByDescending(x => prop.GetValue(x, null));
                    else
                        items = items.OrderBy(x => prop.GetValue(x, null));
                }
            }
            var pagedResult = items
                .Skip((options.Page - 1) * options.PageSize)
                .Take(options.PageSize);

            return new ListProductResult
            {
                Items = pagedResult,
                TotalCount = pagedResult.Count(),
                Page = options.Page,
                PageSize = options.PageSize
            };
        }

        private QueryOptions GetQueryOptions(ListProductQuery request)
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

            return options;
        }
    }
}
