using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents a request to create a new <see cref="Domain.Entities.Sale"/>.
    /// </summary>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        /// <summary>
        /// Gets or sets the sale number (business reference or invoice code).
        /// </summary>
        public string SaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the sale occurred.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the customer making the sale.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the branch where the sale was made.
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Gets or sets the collection of items to include in this sale.
        /// </summary>
        public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();
    }

    /// <summary>
    /// Data transfer object representing a single line item for a new sale.
    /// </summary>
    public class CreateSaleItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product being sold.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product being sold (must be at least 1).
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product (must be greater than zero).
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}

