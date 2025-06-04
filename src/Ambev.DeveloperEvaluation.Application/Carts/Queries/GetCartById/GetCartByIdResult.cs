namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById
{
    /// <summary>
    /// Represents the data returned when querying a cart by its ID, including header information,
    /// calculated total amount, and line items.
    /// </summary>
    public class GetCartByIdResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the cart number (business reference or invoice code).
        /// </summary>
        public string CartNumber { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the cart occurred.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the customer associated with this cart.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the branch where this cart took place.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cart has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the cart, calculated as the sum of each item's net total.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the collection of line items associated with this cart.
        /// </summary>
        public List<CartItemResult> Items { get; set; } = new List<CartItemResult>();
    }

    /// <summary>
    /// Represents a single line item returned when querying a cart, including quantity, pricing, discount, and net total.
    /// </summary>
    public class CartItemResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of this cart item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the product associated with this item.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of this product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product for this line item.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount rate applied to this line item (e.g., 0.10 for 10%).
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this line item has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the net total for this line item, calculated as:
        /// <c>Quantity * UnitPrice * (1 − DiscountRate)</c>.
        /// </summary>
        public decimal Total { get; set; }
    }
}
