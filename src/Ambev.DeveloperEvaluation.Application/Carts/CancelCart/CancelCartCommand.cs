using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart
{
    /// <summary>
    /// Command to request cancellation of a specific <see cref="Domain.Entities.Cart"/>.
    /// </summary>
    public class CancelCartCommand : IRequest<CancelCartResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart to cancel.
        /// </summary>
        public Guid Id { get; set; }
    }
}
