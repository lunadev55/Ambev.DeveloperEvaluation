//using Ambev.DeveloperEvaluation.Domain.Entities;

//namespace Ambev.DeveloperEvaluation.Domain.Repositories
//{
//    public interface ISaleRepository
//    {
//        Task AddAsync(Sale sale, CancellationToken cancellationToken = default);
//        void Update(Sale sale);
//        Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
//        Task<IReadOnlyList<Sale>> ListAsync(int page, int size, CancellationToken cancellationToken = default);
//        Task SaveChangesAsync(CancellationToken cancellationToken = default);

//        /// <summary>
//        /// Deletes all SaleItem rows for the given Sale ID.
//        /// </summary>
//        Task DeleteAllItemsBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default);

//        /// <summary>
//        /// Inserts the provided SaleItem instances in a batch for the given Sale.
//        /// </summary>
//        Task AddItemsAsync(IEnumerable<SaleItem> items, Guid saleId, CancellationToken cancellationToken = default);
//    }
//}

using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for persisting and retrieving <see cref="Sale"/> aggregates,
    /// as well as managing their associated <see cref="SaleItem"/> entities.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Adds a new <see cref="Sale"/> to the underlying data store.
        /// </summary>
        /// <param name="sale">The sale aggregate to add.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task AddAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an existing <see cref="Sale"/> as modified so that changes
        /// will be persisted on the next <see cref="SaveChangesAsync"/> call.
        /// </summary>
        /// <param name="sale">The sale aggregate with updated state.</param>
        void Update(Sale sale);

        /// <summary>
        /// Retrieves a <see cref="Sale"/> by its unique identifier, including any owned items.
        /// </summary>
        /// <param name="id">The unique ID of the sale to fetch.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The <see cref="Sale"/> with the specified ID, or <c>null</c> if not found.
        /// </returns>
        Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of <see cref="Sale"/> aggregates.
        /// </summary>
        /// <param name="page">The page number (1-based) to fetch.</param>
        /// <param name="size">The number of sales per page.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A read-only list of <see cref="Sale"/> instances for the specified page.
        /// </returns>
        Task<IReadOnlyList<Sale>> ListAsync(int page, int size, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists all pending changes (inserts, updates, deletes) to the data store.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all <see cref="SaleItem"/> rows associated with a given <see cref="Sale"/>.
        /// Useful when replacing or clearing items for that sale.
        /// </summary>
        /// <param name="saleId">The ID of the sale whose items should be removed.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task DeleteAllItemsBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a batch of <see cref="SaleItem"/> instances for a specific <see cref="Sale"/>.
        /// </summary>
        /// <param name="items">The collection of items to add to the sale.</param>
        /// <param name="saleId">
        /// The ID of the sale under which these items should be grouped.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        Task AddItemsAsync(IEnumerable<SaleItem> items, Guid saleId, CancellationToken cancellationToken = default);
    }
}

