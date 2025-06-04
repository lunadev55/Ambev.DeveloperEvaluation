//using Ambev.DeveloperEvaluation.Domain.Entities;

//namespace Ambev.DeveloperEvaluation.Domain.Repositories
//{
//    /// <summary>
//    /// Defines CRUD and listing operations for Product aggregates.
//    /// </summary>
//    public interface IProductRepository
//    {
//        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
//        Task<IReadOnlyList<Product>> ListAsync(
//            int page,
//            int size,
//            string orderBy = null,
//            CancellationToken cancellationToken = default);
//        Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
//        void Update(Product product);
//        void Delete(Product product);
//        Task SaveChangesAsync(CancellationToken cancellationToken = default);
//    }
//}

// File: src/Ambev.DeveloperEvaluation.Domain/Repositories/IProductRepository.cs
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Defines CRUD and listing operations for Product aggregates.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The product’s GUID.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The matching Product, or null if not found.</returns>
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated (and optionally ordered) list of products.
        /// </summary>
        /// <param name="page">1-based page number.</param>
        /// <param name="size">Number of items per page.</param>
        /// <param name="orderBy">
        /// A comma-separated “prop [asc|desc]” string (e.g. “price desc,name asc”).
        /// If null or empty, defaults to ordering by Id ascending.
        /// </param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A read-only list of Products for that page.</returns>
        Task<IReadOnlyList<Product>> ListAsync(
            int page,
            int size,
            string orderBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new product to the context (pending save).
        /// </summary>
        /// <param name="product">The Product to insert.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The same Product instance (with Id populated if generated).</returns>
        Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an existing product as updated in the context.
        /// </summary>
        /// <param name="product">The Product with updated data.</param>
        void Update(Product product);

        /// <summary>
        /// Marks an existing product for deletion in the context.
        /// </summary>
        /// <param name="product">The Product to remove.</param>
        void Delete(Product product);

        /// <summary>
        /// Persists any pending changes (Add/Update/Delete) to the database.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

