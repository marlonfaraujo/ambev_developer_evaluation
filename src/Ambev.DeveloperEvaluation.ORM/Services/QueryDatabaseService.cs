using Ambev.DeveloperEvaluation.ORM.Dtos.Branch;
using Ambev.DeveloperEvaluation.ORM.Dtos.Product;
using Ambev.DeveloperEvaluation.ORM.Dtos.Sale;
using Ambev.DeveloperEvaluation.ORM.Queries;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;

namespace Ambev.DeveloperEvaluation.ORM.Services
{
    public class QueryDatabaseService
    {
        private readonly DefaultContext _context;

        public QueryDatabaseService(DefaultContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListSalesQueryResult>> ListSalesAsync(ListSalesQueryParams queryParameters)
        {
            var query = new StringBuilder();
            query.AppendLine(ListSalesQuery.SELECT);

            var sqlParameters = new List<NpgsqlParameter>();
            if (queryParameters != null)
            {
                if (!string.IsNullOrWhiteSpace(queryParameters.SaleId))
                {
                    query.AppendLine(@"AND s.""Id"" = @saleId");
                    sqlParameters.Add(new NpgsqlParameter("saleId", queryParameters.SaleId));
                }
                if (!string.IsNullOrWhiteSpace(queryParameters.UserId))
                {
                    query.AppendLine(@"AND s.""UserId"" = @userId");
                    sqlParameters.Add(new NpgsqlParameter("userId", queryParameters.UserId));
                }
                if (queryParameters.SaleNumber > 0)
                {
                    query.AppendLine(@"AND s.""SaleNumber"" = @saleNumber");
                    sqlParameters.Add(new NpgsqlParameter("saleNumber", queryParameters.SaleNumber));
                }
                if (!string.IsNullOrWhiteSpace(queryParameters.ProductId))
                {
                    query.AppendLine(@"AND si.""ProductId"" = @productId");
                    sqlParameters.Add(new NpgsqlParameter("productId", queryParameters.ProductId));
                }
                if (!string.IsNullOrWhiteSpace(queryParameters.BranchId))
                {
                    query.AppendLine(@"AND s.""BranchSaleId"" = @branchId");
                    sqlParameters.Add(new NpgsqlParameter("branchId", queryParameters.BranchId));
                }
            }
            query.AppendLine("LIMIT @pageSize OFFSET @pageSize * (@pageNumber - 1)");
            sqlParameters.Add(new NpgsqlParameter("pageSize", queryParameters.Pager.PageSize));
            sqlParameters.Add(new NpgsqlParameter("pageNumber", queryParameters.Pager.PageNumber));

            var result = await _context.Database.SqlQueryRaw<ListSalesQueryResult>(query.ToString(), 
                sqlParameters.ToArray()).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ListBranchsQueryResult>> ListBranchsAsync(ListBranchsQueryParams queryParameters)
        {
            var query = new StringBuilder();
            query.Append(ListBranchsQuery.SELECT);

            var sqlParameters = new List<NpgsqlParameter>();
            if (queryParameters != null)
            {
                if (!string.IsNullOrWhiteSpace(queryParameters.Name))
                {
                    query.AppendLine(@"AND ""Name"" ILIKE '%'@name'%'");
                    sqlParameters.Add(new NpgsqlParameter("name", queryParameters.Name));
                }
            }
            query.AppendLine("LIMIT @pageSize OFFSET @pageSize * (@pageNumber - 1)");
            sqlParameters.Add(new NpgsqlParameter("pageSize", queryParameters.Pager.PageSize));
            sqlParameters.Add(new NpgsqlParameter("pageNumber", queryParameters.Pager.PageNumber));

            var result = await _context.Database.SqlQueryRaw<ListBranchsQueryResult>(query.ToString(),
                sqlParameters.ToArray()).ToListAsync();


            return result;
        }

        public async Task<IEnumerable<ListProductsQueryResult>> ListProductsAsync(ListProductsQueryParams queryParameters)
        {
            var query = new StringBuilder();
            query.AppendLine(ListProductsQuery.SELECT);

            var sqlParameters = new List<NpgsqlParameter>();
            query.AppendLine("LIMIT @pageSize OFFSET @pageSize * (@pageNumber - 1)");
            if (queryParameters != null)
            {
                if (!string.IsNullOrWhiteSpace(queryParameters.Name))
                {
                    query.AppendLine(@"AND ""Name"" ILIKE '%'@name'%'");
                    sqlParameters.Add(new NpgsqlParameter("name", queryParameters.Name));
                }
            }
            sqlParameters.Add(new NpgsqlParameter("pageSize", queryParameters.Pager.PageSize));
            sqlParameters.Add(new NpgsqlParameter("pageNumber", queryParameters.Pager.PageNumber));

            var result = await _context.Database.SqlQueryRaw<ListProductsQueryResult>(query.ToString(),
                sqlParameters.ToArray()).ToListAsync();

            return result;
        }
    }
}
