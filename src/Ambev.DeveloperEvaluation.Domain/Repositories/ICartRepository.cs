using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for persisting and retrieving <see cref="Cart"/> aggregates,
    /// as well as managing their associated <see cref="CartItem"/> entities.
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Adds a new <see cref="Cart"/> to the underlying data store.
        /// </summary>
        /// <param name="cart">The cart aggregate to add.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task AddAsync(Cart cart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an existing <see cref="Cart"/> as modified so that changes
        /// will be persisted on the next <see cref="SaveChangesAsync"/> call.
        /// </summary>
        /// <param name="cart">The cart aggregate with updated state.</param>
        void Update(Cart cart);

        /// <summary>
        /// Retrieves a <see cref="Cart"/> by its unique identifier, including any owned items.
        /// </summary>
        /// <param name="id">The unique ID of the cart to fetch.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The <see cref="Cart"/> with the specified ID, or <c>null</c> if not found.
        /// </returns>
        Task<Cart> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of <see cref="Cart"/> aggregates.
        /// </summary>
        /// <param name="page">The page number (1-based) to fetch.</param>
        /// <param name="size">The number of carts per page.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A read-only list of <see cref="Cart"/> instances for the specified page.
        /// </returns>
        Task<IReadOnlyList<Cart>> ListAsync(int page, int size, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists all pending changes (inserts, updates, deletes) to the data store.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all <see cref="CartItem"/> rows associated with a given <see cref="Cart"/>.
        /// Useful when replacing or clearing items for that cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart whose items should be removed.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task DeleteAllItemsByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a batch of <see cref="CartItem"/> instances for a specific <see cref="Cart"/>.
        /// </summary>
        /// <param name="items">The collection of items to add to the cart.</param>
        /// <param name="cartId">
        /// The ID of the cart under which these items should be grouped.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task AddItemsAsync(IEnumerable<CartItem> items, Guid cartId, CancellationToken cancellationToken = default);
    }
}

