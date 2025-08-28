using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.ORM.Dtos.Branch;
using Ambev.DeveloperEvaluation.ORM.Dtos.Product;
using Ambev.DeveloperEvaluation.ORM.Dtos.Sale;
using Ambev.DeveloperEvaluation.ORM.Dtos.User;
using Ambev.DeveloperEvaluation.ORM.Queries;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Reflection;
using System.Text;

namespace Ambev.DeveloperEvaluation.ORM.Services
{
    public class QueryDatabaseService : IQueryDatabaseService
    {
        private readonly DefaultContext _context;

        public QueryDatabaseService(DefaultContext context)
        {
            _context = context;
        }

        public async Task<bool> BranchInSales(Guid branchId)
        {
            var query = new StringBuilder();
            query.Append(@"
                SELECT
                    b.*
                FROM public.""Sales"" s
                    INNER JOIN public.""Branchs"" b ON s.""BranchSaleId"" = b.""Id""
                WHERE s.""BranchSaleId"" = b.""Id""    
            ");

            var sqlParameters = new List<NpgsqlParameter>();

            if (Guid.Empty != branchId)
            {
                query.AppendLine(@"AND s.""BranchSaleId"" = @branchId");
                sqlParameters.Add(new NpgsqlParameter("branchId", branchId));
            }

            var result = await _context.Database.SqlQueryRaw<ListBranchsQueryResult>(query.ToString(), sqlParameters.ToArray()).AnyAsync();

            return result;
        }

        public async Task<bool> UserInSales(Guid userId)
        {
            var query = new StringBuilder();
            query.Append(@"
                SELECT
                    u.*
                FROM public.""Sales"" s
                    INNER JOIN public.""Users"" u ON s.""UserId"" = u.""Id""
                WHERE s.""UserId"" = u.""Id""    
            ");

            var sqlParameters = new List<NpgsqlParameter>();

            if (Guid.Empty != userId)
            {
                query.AppendLine(@"AND s.""UserId"" = @userId");
                sqlParameters.Add(new NpgsqlParameter("userId", userId));
            }

            var result = await _context.Database.SqlQueryRaw<ListUserQueryResult>(query.ToString(), sqlParameters.ToArray()).AnyAsync();

            return result;
        }

        public async Task<bool> ProductInSaleItems(Guid productId)
        {
            var query = new StringBuilder();
            query.Append(@"
                SELECT
                    p.*
                FROM public.""SaleItems"" si
                    INNER JOIN public.""Products"" p ON si.""ProductId"" = p.""Id""
                WHERE si.""ProductId"" = p.""Id""  
            ");

            var sqlParameters = new List<NpgsqlParameter>();

            if (Guid.Empty != productId)
            {
                query.AppendLine(@"AND si.""ProductId"" = @productId");
                sqlParameters.Add(new NpgsqlParameter("productId", productId));
            }

            var result = await _context.Database.SqlQueryRaw<ListProductsQueryResult>(query.ToString(), sqlParameters.ToArray()).AnyAsync();
            return result;

        }

        public async Task<IEnumerable<TEntity>> GetSaleQueryById<TEntity>(Guid saleId) where TEntity : class
        {
            var sqlQueryParameters = ListSalesSqlQuery.GetSqlQuery(new ListSalesQueryParams()
            {
                SaleId = saleId
            });
            var result = await Select<TEntity>(sqlQueryParameters.QuerySql, sqlQueryParameters.SqlParameters.ToArray());
            return result;
        }

        public async Task<IEnumerable<TEntity>> Select<TEntity>(string query, params object[] parameters) where TEntity : class
        {
            var result = await _context.Database.SqlQueryRaw<TEntity>(query, parameters).ToListAsync();
            return result;
        }

        public object[] GetSqlParameters<T>(T parameters)
        {
            return ToSqlParameters(parameters).ToArray();
        }

        private List<NpgsqlParameter> ToSqlParameters(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var parameters = new List<NpgsqlParameter>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value == null) 
                    continue;
                
                var parameter = new NpgsqlParameter($"@{prop.Name}", value);
                parameters.Add(parameter);
            }

            return parameters;
        }
    }
}
