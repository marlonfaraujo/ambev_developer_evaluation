using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications
{
    public class MaxQuantityProductItemsSpecification : ISpecification<IEnumerable<SaleItem>>
    {
        public const int MAX_ITEMS_PER_PRODUCT = 20;
        
        public bool IsSatisfiedBy(IEnumerable<SaleItem> items)
        {
            if (items.Any(x => x.ProductItemQuantity > MAX_ITEMS_PER_PRODUCT)) return true;
            return false;
        }
    }   
}
