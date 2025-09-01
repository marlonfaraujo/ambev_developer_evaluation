using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
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
        }

        public ProductChangedEvent CreateProductChangedEvent()
        {
            return new ProductChangedEvent(this.Id, this.Name);
        }
    }
}
