using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.ORM.Dtos.Branch
{
    public class ListBranchsQueryParams
    {
        public ListBranchsQueryParams()
        {
            Pager = new Pager();
        }

        public string Name { get; set; } = string.Empty;
        public Pager Pager { get; set; }
    }
}
