namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class BranchChangedEvent : IDomainNotification
    {
        public Guid BranchId { get; }
        public string BranchName { get; }
        public BranchChangedEvent(Guid branchId, string branchName)
        {
            BranchId = branchId;
            BranchName = branchName;
        }
    }
}
