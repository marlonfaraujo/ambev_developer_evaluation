using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.ORM.Dtos.Product
{
    public class ListProductsQueryParams
    {
        public ListProductsQueryParams()
        {
            Pager = new Pager();
        }

        public string Name { get; set; } = string.Empty;
        public Pager Pager { get; set; }
    }
}
