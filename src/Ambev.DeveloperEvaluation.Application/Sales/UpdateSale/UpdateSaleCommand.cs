using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Represents a request to update an existing <see cref="Domain.Entities.Sale"/>,
    /// including header fields and a collection of updated line items.
    /// </summary>
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to update.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the new sale number (business reference or invoice code).
        /// </summary>
        public string SaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the updated date and time for the sale.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer associated with the sale.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name (plain text) of the branch where the sale occurred.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="UpdateSaleItemDto"/> instances
        /// that represent the updated line items for this sale.
        /// </summary>
        public List<UpdateSaleItemDto> Items { get; set; } = new List<UpdateSaleItemDto>();
    }

    /// <summary>
    /// Data transfer object representing a single line item for an update operation.
    /// </summary>
    public class UpdateSaleItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product for this line item.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the updated quantity of the product (must be at least 1).
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the updated unit price of the product (must be greater than zero).
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}

