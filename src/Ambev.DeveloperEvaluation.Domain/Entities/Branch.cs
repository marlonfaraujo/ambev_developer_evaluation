using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Branch(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public Branch()
        {
        }

        public BranchChangedEvent CreateBranchChangedEvent()
        {
            return new BranchChangedEvent(this.Id, this.Name);
        }
    }
}
