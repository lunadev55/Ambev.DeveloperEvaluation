namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class BranchId
    {
        protected BranchId() { }
        public Guid Value { get; private set; }
        public BranchId(Guid value) => Value = value;
    }
}
