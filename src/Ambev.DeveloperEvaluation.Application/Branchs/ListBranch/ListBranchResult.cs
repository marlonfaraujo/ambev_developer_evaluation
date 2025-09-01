namespace Ambev.DeveloperEvaluation.Application.Branchs.ListBranch
{
    public class ListBranchResult
    {
        public IEnumerable<ListBranchResultData> Items { get; set; }
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
    public class ListBranchResultData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
