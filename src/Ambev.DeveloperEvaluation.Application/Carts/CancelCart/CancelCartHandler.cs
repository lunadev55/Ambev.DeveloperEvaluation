using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart
{
    /// <summary>
    /// Handles the cancellation of a <see cref="Domain.Entities.Cart"/> by marking it as cancelled.
    /// </summary>
    public class CancelCartHandler : IRequestHandler<CancelCartCommand, CancelCartResult>
    {
        private readonly ICartRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelCartHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to load and save <see cref="Domain.Entities.Cart"/> aggregates.
        /// </param>
        public CancelCartHandler(ICartRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Processes a <see cref="CancelCartCommand"/> by retrieving the specified cart,
        /// invoking its cancellation logic, and persisting the change.
        /// </summary>
        /// <param name="request">
        /// The <see cref="CancelCartCommand"/> containing the ID of the cart to cancel.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while awaiting the database operations.
        /// </param>
        /// <returns>
        /// A <see cref="CancelCartResult"/> indicating whether the cancellation succeeded.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no cart with the specified ID exists in the repository.
        /// </exception>
        public async Task<CancelCartResult> Handle(CancelCartCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the cart from the repository
            var cart = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID '{request.Id}' not found.");

            // Invoke domain logic to cancel the cart (and its items)
            cart.CancelCart();

            // Mark the cart as modified so EF Core will persist the IsCancelled change
            _repository.Update(cart);

            // Save changes to the database
            await _repository.SaveChangesAsync(cancellationToken);

            return new CancelCartResult { Success = true };
        }
    }
}
