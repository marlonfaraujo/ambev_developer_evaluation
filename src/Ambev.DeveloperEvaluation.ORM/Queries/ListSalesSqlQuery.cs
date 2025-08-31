using Ambev.DeveloperEvaluation.ORM.Dtos;
using Ambev.DeveloperEvaluation.ORM.Dtos.Sale;
using Npgsql;
using System.Text;

namespace Ambev.DeveloperEvaluation.ORM.Queries
{
    public class ListSalesSqlQuery
    {
        public const string SELECT = @"
            SELECT
                s.""Id"" AS SaleId,
                s.""SaleNumber"",
                s.""SaleDate"",
                s.""TotalSalePrice"",
                s.""SaleStatus"",
                u.""Id"" AS UserId,
                u.""Username"" AS UserName,
                b.""Id"" AS BranchId,
                b.""Name"" AS BranchName,
                b.""Description"" AS BranchDescription,
                si.""Id"" AS SaleItemId,
                si.""ProductItemQuantity"",
                si.""UnitProductItemPrice"",
                si.""DiscountAmount"",
                si.""TotalSaleItemPrice"",
                si.""TotalWithoutDiscount"",
                si.""SaleItemStatus"",
                p.""Id"" AS ProductId,
                p.""Name"" AS ProductName,
                p.""Description"" AS ProductDescription,
                p.""Price"" AS ProductPrice
            FROM public.""Sales"" s
                INNER JOIN public.""Users"" u ON s.""UserId"" = u.""Id""
                INNER JOIN public.""Branchs"" b ON s.""BranchSaleId"" = b.""Id""
                INNER JOIN public.""SaleItems"" si ON si.""SaleId"" = s.""Id""
                INNER JOIN public.""Products"" p ON si.""ProductId"" = p.""Id""
            WHERE s.""UserId"" = u.""Id""
        ";

        public static SqlQueryParams<NpgsqlParameter> GetSqlQuery(ListSalesQueryParams queryParameters)
        {
            var query = new StringBuilder();
            query.Append(SELECT);

            var sqlParameters = new List<NpgsqlParameter>();
            if (queryParameters != null)
            {
                if (queryParameters.SaleId.HasValue)
                {
                    query.AppendLine(@"AND s.""Id"" = @saleId");
                    sqlParameters.Add(new NpgsqlParameter("saleId", queryParameters.SaleId));
                }
                if (queryParameters.UserId.HasValue)
                {
                    query.AppendLine(@"AND s.""UserId"" = @userId");
                    sqlParameters.Add(new NpgsqlParameter("userId", queryParameters.UserId));
                }
                if (queryParameters.SaleNumber > 0)
                {
                    query.AppendLine(@"AND s.""SaleNumber"" = @saleNumber");
                    sqlParameters.Add(new NpgsqlParameter("saleNumber", queryParameters.SaleNumber));
                }
                if (queryParameters.ProductId.HasValue)
                {
                    query.AppendLine(@"AND si.""ProductId"" = @productId");
                    sqlParameters.Add(new NpgsqlParameter("productId", queryParameters.ProductId));
                }
                if (queryParameters.BranchId.HasValue)
                {
                    query.AppendLine(@"AND s.""BranchSaleId"" = @branchId");
                    sqlParameters.Add(new NpgsqlParameter("branchId", queryParameters.BranchId));
                }
                if (queryParameters.SaleItemId.HasValue)
                {
                    query.AppendLine(@"AND si.""Id"" = @saleItemId");
                    sqlParameters.Add(new NpgsqlParameter("saleItemId", queryParameters.SaleItemId));
                }
            }
            query.AppendLine(@"ORDER BY s.""SaleDate"" DESC");
            query.AppendLine("LIMIT @pageSize OFFSET @pageSize * (@pageNumber - 1)");
            sqlParameters.Add(new NpgsqlParameter("pageSize", queryParameters.Pager.PageSize));
            sqlParameters.Add(new NpgsqlParameter("pageNumber", queryParameters.Pager.PageNumber));

            return new SqlQueryParams<NpgsqlParameter>
            {
                QuerySql = query.ToString(),
                SqlParameters = sqlParameters
            };
        }
    }
}
