namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; }
        public CustomerId CustomerId { get; private set; }
        public BranchId BranchId { get; private set; }
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
        private readonly List<SaleItem> _items;
        public bool IsCancelled { get; private set; }

        public Sale(Guid id, string saleNumber, DateTime date, CustomerId customerId, BranchId branchId)
        {
            Id = id;
            SaleNumber = saleNumber ?? throw new ArgumentNullException(nameof(saleNumber));
            Date = date;
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
            BranchId = branchId ?? throw new ArgumentNullException(nameof(branchId));
            _items = new List<SaleItem>();
            IsCancelled = false;
        }

        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (IsCancelled)
                throw new InvalidOperationException("Cannot add items to a cancelled sale.");

            var discount = CalculateDiscount(quantity);
            var item = new SaleItem(Guid.NewGuid(), productId, quantity, unitPrice, discount);
            _items.Add(item);
        }

        public void CancelSale()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Sale is already cancelled.");
            IsCancelled = true;
            foreach (var item in _items)
            {
                item.CancelItem();
            }
        }

        public decimal TotalAmount()
        {
            return _items.Sum(i => i.Total);
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0m;
            if (quantity >= 4 && quantity < 10)
                return 0.10m;
            if (quantity >= 10 && quantity <= 20)
                return 0.20m;

            throw new DomainException("Cannot sell more than 20 identical items.");
        }
    }
}
