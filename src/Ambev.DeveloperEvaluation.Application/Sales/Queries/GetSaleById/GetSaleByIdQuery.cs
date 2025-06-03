using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    /// <summary>
    /// Represents a request to retrieve a <see cref="Domain.Entities.Sale"/> by its unique identifier.
    /// </summary>
    public class GetSaleByIdQuery : IRequest<GetSaleByIdResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to retrieve.
        /// </summary>
        public Guid Id { get; set; }
    }
}
