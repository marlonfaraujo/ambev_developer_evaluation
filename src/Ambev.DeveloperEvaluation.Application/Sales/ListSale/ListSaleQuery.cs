using Ambev.DeveloperEvaluation.Application.Requests;
using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleQuery : IRequestApplication<ListSaleResult>
    {
        public Guid? SaleId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? ProductId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = string.Empty; 
        public string SortDirection { get; set; } = string.Empty;
    }
}
