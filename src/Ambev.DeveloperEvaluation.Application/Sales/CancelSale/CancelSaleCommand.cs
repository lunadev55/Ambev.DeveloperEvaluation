using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Command to request cancellation of a specific <see cref="Domain.Entities.Sale"/>.
    /// </summary>
    public class CancelSaleCommand : IRequest<CancelSaleResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to cancel.
        /// </summary>
        public Guid Id { get; set; }
    }
}
