using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="ICartRepository"/> using <see cref="DefaultContext"/>.
    /// Provides data access for <see cref="Cart"/> aggregates, including header updates
    /// and raw‐SQL operations for child <see cref="CartItem"/> collections.
    /// </summary>
    public class CartRepository : ICartRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class.
        /// </summary>
        /// <param name="context">The EF Core <see cref="DefaultContext"/> to use.</param>
        public CartRepository(DefaultContext context) => _context = context;

        /// <summary>
        /// Adds a new <see cref="Cart"/> to the EF Core change‐tracker.
        /// Actual database insertion occurs when <see cref="SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="cart">The <see cref="Cart"/> entity to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            _context.Carts.Add(cart);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Marks an existing <see cref="Cart"/> as modified so that its header properties
        /// (e.g., <c>CartNumber</c>, <c>Date</c>, <c>CustomerId</c>, <c>BranchId</c>) will be updated.
        /// Child <see cref="CartItem"/> rows are handled separately via raw‐SQL methods.
        /// </summary>
        /// <param name="cart">The <see cref="Cart"/> entity with updated properties.</param>
        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
        }

        /// <summary>
        /// Retrieves a non‐cancelled <see cref="Cart"/> by its primary key, including its child items.
        /// </summary>
        /// <param name="id">The <see cref="Cart"/> identifier (GUID).</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        /// <returns>
        /// The matching <see cref="Cart"/> (with its <see cref="Cart.Items"/> collection), or <c>null</c>
        /// if no matching, non‐cancelled cart exists.
        /// </returns>
        public async Task<Cart> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Where(s => s.IsCancelled == false)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <summary>
        /// Returns a paginated list of <see cref="Cart"/> entities, ordered by <c>Date</c>.
        /// </summary>
        /// <param name="page">The 1‐based page number.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        /// <returns>A read‐only list of <see cref="Cart"/> entities for the requested page.</returns>
        public async Task<IReadOnlyList<Cart>> ListAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .OrderBy(s => s.Date)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Saves all pending changes in the <see cref="DefaultContext"/> to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes all child <see cref="CartItem"/> rows for a given <see cref="Cart"/> ID using raw SQL.
        /// </summary>
        /// <param name="cartId">The parent <see cref="Cart"/> identifier.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task DeleteAllItemsByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                DELETE FROM ""CartItems""
                WHERE ""CartId"" = {0};";
                        
            await _context.Database.ExecuteSqlRawAsync(
                sql,
                new object[] { cartId },
                cancellationToken
            );
        }

        /// <summary>
        /// Inserts a collection of new <see cref="CartItem"/> rows for the specified <see cref="Cart"/> ID
        /// using raw SQL. Assumes each <see cref="CartItem"/> in <paramref name="items"/> has a valid GUID
        /// and pre‐computed <c>DiscountRate</c>.
        /// </summary>
        /// <param name="items">The list of <see cref="CartItem"/> entities to insert.</param>
        /// <param name="cartId">The parent <see cref="Cart"/> identifier (foreign key).</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task AddItemsAsync(IEnumerable<CartItem> items, Guid cartId, CancellationToken cancellationToken = default)
        {
            const string insertSql = @"
                INSERT INTO ""CartItems"" (
                    ""Id"",
                    ""ProductId"",
                    ""Quantity"",
                    ""UnitPrice"",
                    ""DiscountRate"",
                    ""IsCancelled"",
                    ""CartId""
                ) VALUES (
                    {0},  -- CartItem.Id
                    {1},  -- ProductId
                    {2},  -- Quantity
                    {3},  -- UnitPrice
                    {4},  -- DiscountRate
                    FALSE,
                    {5}   -- CartId (foreign key)
                );";

            foreach (var item in items)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    insertSql,
                    new object[]
                    {
                        item.Id,
                        item.ProductId,
                        item.Quantity,
                        item.UnitPrice,
                        item.DiscountRate,
                        cartId
                    },
                    cancellationToken
                );
            }
        }
    }
}
