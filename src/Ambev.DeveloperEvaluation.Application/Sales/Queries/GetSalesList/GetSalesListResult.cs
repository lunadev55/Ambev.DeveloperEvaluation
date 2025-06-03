namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    /// <summary>
    /// Represents the paginated result for a list of sales,
    /// including the current page, page size, and the collection of sales items.
    /// </summary>
    public class GetSalesListResult
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
        /// Gets or sets the collection of <see cref="SalesListItem"/> DTOs for this page.
        /// </summary>
        public List<SalesListItem> Items { get; set; } = new List<SalesListItem>();
    }

    /// <summary>
    /// Represents a single sale entry in a paginated list,
    /// including basic header information and the calculated total.
    /// </summary>
    public class SalesListItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number (business reference or invoice code).
        /// </summary>
        public string SaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the sale occurred.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the sale, calculated as the sum of all items.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sale has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
