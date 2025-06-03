namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a line item within a <see cref="Sale"/>, including product, quantity, pricing,
    /// discount, and cancellation status.
    /// </summary>
    public class SaleItem
    {
        /// <summary>
        /// Gets the unique identifier for this sale item.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the product associated with this item.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the parent <see cref="Sale"/> to which this item belongs.
        /// </summary>
        public Guid SaleId { get; private set; }

        /// <summary>
        /// Gets the number of units of the product in this line item.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the unit price of the product in this line item.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Gets the discount rate applied to this line item (e.g., 0.10 for 10%).
        /// </summary>
        public decimal DiscountRate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this item has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Gets the total amount for this line item, calculated as:
        /// <c>Quantity × UnitPrice × (1 − DiscountRate)</c>.
        /// </summary>
        public decimal Total => Quantity * UnitPrice * (1 - DiscountRate);

        /// <summary>
        /// Protected constructor used by EF Core for materialization.
        /// </summary>
        protected SaleItem() { }

        /// <summary>
        /// Constructs a new <see cref="SaleItem"/> with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier for this sale item (must be non-empty GUID).</param>
        /// <param name="productId">The unique identifier of the product (must be non-empty GUID).</param>
        /// <param name="quantity">The quantity of product (must be ≥ 1 and ≤ 20).</param>
        /// <param name="unitPrice">The unit price (must be > 0).</param>
        /// <param name="discountRate">
        /// The discount rate to apply (e.g., 0.10 for 10%). Domain logic ensures valid range.
        /// </param>
        /// <exception cref="DomainException">
        /// Thrown if <paramref name="id"/> or <paramref name="productId"/> is empty,
        /// if <paramref name="quantity"/> &lt; 1 or &gt; 20, or if <paramref name="unitPrice"/> ≤ 0.
        /// </exception>
        public SaleItem(Guid id, Guid productId, int quantity, decimal unitPrice, decimal discountRate)
        {
            if (id == Guid.Empty)
                throw new DomainException("SaleItem ID must be a valid GUID.");

            if (productId == Guid.Empty)
                throw new DomainException("ProductId must be a valid GUID.");

            if (quantity < 1)
                throw new DomainException("Quantity must be at least 1.");

            if (unitPrice <= 0)
                throw new DomainException("UnitPrice must be greater than zero.");

            if (quantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items.");

            Id = id;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountRate = discountRate;
            IsCancelled = false;
        }

        /// <summary>
        /// Sets the <see cref="SaleId"/> for this item. Intended to be called by the <see cref="Sale"/> aggregate.
        /// </summary>
        /// <param name="saleId">The identifier of the parent sale.</param>
        internal void SetSaleId(Guid saleId)
        {
            SaleId = saleId;
        }

        /// <summary>
        /// Marks this line item as cancelled. Once cancelled, it cannot be uncancelled.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the item is already cancelled.</exception>
        public void CancelItem()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Item is already cancelled.");

            IsCancelled = true;
        }
    }
}



