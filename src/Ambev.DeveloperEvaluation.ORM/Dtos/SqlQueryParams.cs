namespace Ambev.DeveloperEvaluation.ORM.Dtos
{
    public class SqlQueryParams<T>
    {
        public string QuerySql { get; set; }
        public IEnumerable<T> SqlParameters { get; set; }
    }
}
