using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Dtos.Branch;
using Ambev.DeveloperEvaluation.ORM.Dtos.Product;
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

        public async Task<bool> BranchsInSale(Guid branchId)
        {
            var query = new StringBuilder();
            query.Append(@"
                SELECT
                    s.*
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

        public async Task<bool> ProductsInSaleItems(Guid productId)
        {
            var query = new StringBuilder();
            query.Append(@"
                SELECT
                    si.*
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
