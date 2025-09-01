using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Money Price { get; set; }
        public Product(Guid id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = new Money(price);
        }
        public Product()
        {
            Price = new Money(0);
        }

        public ProductChangedEvent CreateProductChangedEvent()
        {
            return new ProductChangedEvent(this.Id, this.Name);
        }

        public ProductCreatedEvent CreateProductCreatedEvent()
        {
            return new ProductCreatedEvent(this.Id);
        }

        public ProductDeletedEvent CreateProductDeletedEvent()
        {
            return new ProductDeletedEvent(this.Id);
        }
    }
}
