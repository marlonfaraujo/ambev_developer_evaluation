using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Product(string id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
