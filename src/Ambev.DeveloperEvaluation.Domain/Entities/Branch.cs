using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Branch(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public Branch()
        {
        }
    }
}
