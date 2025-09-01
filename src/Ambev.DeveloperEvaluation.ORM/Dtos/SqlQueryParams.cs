namespace Ambev.DeveloperEvaluation.ORM.Dtos
{
    public class SqlQueryParams<T>
    {
        public string QuerySql { get; set; } = string.Empty;
        public IEnumerable<T> SqlParameters { get; set; } = new List<T>();
    }
}
