namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ProductDeletedEvent : IDomainNotification
    {
        public Guid ProductId { get; }
        public ProductDeletedEvent(Guid productId)
        {
            ProductId = productId;
        }
    }
}
