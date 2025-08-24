namespace Ambev.DeveloperEvaluation.ORM.Dtos
{
    public class Pager
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Pager() 
        { 
            PageNumber = 1;
            PageSize = 10;
        }
        public Pager(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 1 ? 10 : pageSize;
        }
    }
}
