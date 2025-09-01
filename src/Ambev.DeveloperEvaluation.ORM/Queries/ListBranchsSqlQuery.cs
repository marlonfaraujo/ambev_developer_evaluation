using Ambev.DeveloperEvaluation.ORM.Dtos;
using Ambev.DeveloperEvaluation.ORM.Dtos.Branch;
using Npgsql;
using System.Text;

namespace Ambev.DeveloperEvaluation.ORM.Queries
{
    public class ListBranchsSqlQuery
    {
        public const string SELECT = @"
            SELECT
                *
            FROM public.""Branchs""
            WHERE ""Id"" = ""Id""
        ";

        public static SqlQueryParams<NpgsqlParameter> GetSqlQuery(ListBranchsQueryParams queryParameters)
        {
            var query = new StringBuilder();
            query.Append(SELECT);

            var sqlParameters = new List<NpgsqlParameter>();
            if (queryParameters != null)
            {
                if (!string.IsNullOrWhiteSpace(queryParameters.Name))
                {
                    query.AppendLine(@"AND ""Name"" ILIKE @name");
                    sqlParameters.Add(new NpgsqlParameter("name", $"%{queryParameters.Name}%"));
                }
            }
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