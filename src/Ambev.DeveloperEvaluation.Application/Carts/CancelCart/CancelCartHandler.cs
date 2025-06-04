using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart
{
    /// <summary>
    /// Handles the cancellation of a <see cref="Domain.Entities.Cart"/> by marking it as cancelled.
    /// </summary>
    public class CancelCartHandler : IRequestHandler<CancelCartCommand, CancelCartResult>
    {
        private readonly ICartRepository _repository;
        private readonly ILogger<CancelCartHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelCartHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to load and save <see cref="Domain.Entities.Cart"/> aggregates.
        /// </param>
        /// /// <param name="logger">
        /// Used to log the Cancel event.
        /// </param>
        public CancelCartHandler(
            ICartRepository repository, ILogger<CancelCartHandler> logger)
        {
            _repository = repository;
            _logger = logger;
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
            var cart = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID '{request.Id}' not found.");
            
            cart.CancelCart();
            
            _repository.Update(cart);
            
            await _repository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "SaleCancelled | SaleId={SaleId}",
                cart.Id
            );
            
            foreach (var item in cart.Items)
            {
                _logger.LogInformation(
                    "  ItemCancelled | CartId={CartId} | ItemId={ItemId} | ProductId={ProductId}",
                    cart.Id,
                    item.Id,
                    item.ProductId
                );
            }

            return new CancelCartResult { Success = true };
        }
    }
}
