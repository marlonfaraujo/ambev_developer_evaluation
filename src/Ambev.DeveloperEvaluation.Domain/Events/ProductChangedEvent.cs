namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ProductChangedEvent : IDomainNotification
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public ProductChangedEvent(Guid productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
        }
    }
}
