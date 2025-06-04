using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Represents a request to update an existing <see cref="Domain.Entities.Cart"/>,
    /// including header fields and a collection of updated line items.
    /// </summary>
    public class UpdateCartCommand : IRequest<UpdateCartResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart to update.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the new cart number (business reference or invoice code).
        /// </summary>
        public string CartNumber { get; set; }

        /// <summary>
        /// Gets or sets the updated date and time for the cart.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer associated with the cart.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name (plain text) of the branch where the cart occurred.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="UpdateCartItemDto"/> instances
        /// that represent the updated line items for this cart.
        /// </summary>
        public List<UpdateCartItemDto> Items { get; set; } = new List<UpdateCartItemDto>();
    }

    /// <summary>
    /// Data transfer object representing a single line item for an update operation.
    /// </summary>
    public class UpdateCartItemDto
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

