namespace Ambev.DeveloperEvaluation.ORM.Dtos.Branch
{
    public class ListBranchsQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
