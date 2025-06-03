using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Handles the cancellation of a <see cref="Domain.Entities.Sale"/> by marking it as cancelled.
    /// </summary>
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelSaleHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to load and save <see cref="Domain.Entities.Sale"/> aggregates.
        /// </param>
        public CancelSaleHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Processes a <see cref="CancelSaleCommand"/> by retrieving the specified sale,
        /// invoking its cancellation logic, and persisting the change.
        /// </summary>
        /// <param name="request">
        /// The <see cref="CancelSaleCommand"/> containing the ID of the sale to cancel.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while awaiting the database operations.
        /// </param>
        /// <returns>
        /// A <see cref="CancelSaleResult"/> indicating whether the cancellation succeeded.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no sale with the specified ID exists in the repository.
        /// </exception>
        public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the sale from the repository
            var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID '{request.Id}' not found.");

            // Invoke domain logic to cancel the sale (and its items)
            sale.CancelSale();

            // Mark the sale as modified so EF Core will persist the IsCancelled change
            _repository.Update(sale);

            // Save changes to the database
            await _repository.SaveChangesAsync(cancellationToken);

            return new CancelSaleResult { Success = true };
        }
    }
}
