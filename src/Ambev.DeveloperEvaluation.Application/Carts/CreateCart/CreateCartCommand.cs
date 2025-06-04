using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Represents a request to create a new <see cref="Domain.Entities.Cart"/>.
    /// </summary>
    public class CreateCartCommand : IRequest<CreateCartResult>
    {
        /// <summary>
        /// Gets or sets the cart number (business reference or invoice code).
        /// </summary>
        public string CartNumber { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the cart occurred.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the customer making the cart.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the branch where the cart was made.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the collection of items to include in this cart.
        /// </summary>
        public List<CreateCartItemDto> Items { get; set; } = new List<CreateCartItemDto>();
    }

    /// <summary>
    /// Data transfer object representing a single line item for a new cart.
    /// </summary>
    public class CreateCartItemDto
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

