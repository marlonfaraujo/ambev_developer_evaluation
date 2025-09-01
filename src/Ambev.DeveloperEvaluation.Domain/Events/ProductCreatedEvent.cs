namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ProductCreatedEvent : IDomainNotification
    {
        public Guid ProductId { get; }
        public ProductCreatedEvent(Guid productId)
        {
            ProductId = productId;
        }
    }
}
