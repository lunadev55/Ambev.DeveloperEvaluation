namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal DiscountRate { get; private set; }
        public bool IsCancelled { get; private set; }
        public decimal Total => Quantity * UnitPrice * (1 - DiscountRate);

        public SaleItem(Guid id, Guid productId, int quantity, decimal unitPrice, decimal discountRate)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");
            if (unitPrice < 0)
                throw new DomainException("Unit price cannot be negative.");

            Id = id;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountRate = discountRate;
            IsCancelled = false;
        }

        public void CancelItem()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Item is already cancelled.");
            IsCancelled = true;
        }
    }
}
