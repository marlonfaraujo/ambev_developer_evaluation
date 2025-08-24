namespace Ambev.DeveloperEvaluation.ORM.Queries
{
    public class ListProductsQuery
    {
        public const string SELECT = @"
            SELECT
                *
            FROM public.""Products""
            WHERE ""Id"" = ""Id""
        ";
    }
}
