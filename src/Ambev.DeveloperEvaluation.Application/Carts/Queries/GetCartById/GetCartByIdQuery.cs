using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById
{
    /// <summary>
    /// Represents a request to retrieve a <see cref="Domain.Entities.Cart"/> by its unique identifier.
    /// </summary>
    public class GetCartByIdQuery : IRequest<GetCartByIdResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart to retrieve.
        /// </summary>
        public Guid Id { get; set; }
    }
}
