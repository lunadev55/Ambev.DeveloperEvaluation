namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartsList
{
    /// <summary>
    /// Represents the paginated result for a list of carts,
    /// including the current page, page size, and the collection of carts items.
    /// </summary>
    public class GetCartsListResult
    {
        /// <summary>
        /// Gets or sets the current page number (1-based).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items returned per page.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="CartsListItem"/> DTOs for this page.
        /// </summary>
        public List<CartsListItem> Items { get; set; } = new List<CartsListItem>();
    }

    /// <summary>
    /// Represents a single carts entry in a paginated list,
    /// including basic header information and the calculated total.
    /// </summary>
    public class CartsListItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the carts.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the carts number (business reference or invoice code).
        /// </summary>
        public string CartNumber { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the carts occurred.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the carts, calculated as the sum of all items.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the carts has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
