namespace Ambev.DeveloperEvaluation.ORM.Queries
{
    public class ListBranchsQuery
    {
        public const string SELECT = @"
            SELECT
                *
            FROM public.""Branchs""
            WHERE ""Id"" = ""Id""
        ";
    }
}
