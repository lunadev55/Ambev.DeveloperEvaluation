using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="ISaleRepository"/> using <see cref="DefaultContext"/>.
    /// Provides data access for <see cref="Sale"/> aggregates, including header updates
    /// and raw‐SQL operations for child <see cref="SaleItem"/> collections.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleRepository"/> class.
        /// </summary>
        /// <param name="context">The EF Core <see cref="DefaultContext"/> to use.</param>
        public SaleRepository(DefaultContext context) => _context = context;

        /// <summary>
        /// Adds a new <see cref="Sale"/> to the EF Core change‐tracker.
        /// Actual database insertion occurs when <see cref="SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="sale">The <see cref="Sale"/> entity to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Add(sale);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Marks an existing <see cref="Sale"/> as modified so that its header properties
        /// (e.g., <c>SaleNumber</c>, <c>Date</c>, <c>CustomerId</c>, <c>BranchId</c>) will be updated.
        /// Child <see cref="SaleItem"/> rows are handled separately via raw‐SQL methods.
        /// </summary>
        /// <param name="sale">The <see cref="Sale"/> entity with updated properties.</param>
        public void Update(Sale sale)
        {
            _context.Sales.Update(sale);
        }

        /// <summary>
        /// Retrieves a non‐cancelled <see cref="Sale"/> by its primary key, including its child items.
        /// </summary>
        /// <param name="id">The <see cref="Sale"/> identifier (GUID).</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        /// <returns>
        /// The matching <see cref="Sale"/> (with its <see cref="Sale.Items"/> collection), or <c>null</c>
        /// if no matching, non‐cancelled sale exists.
        /// </returns>
        public async Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Where(s => s.IsCancelled == false)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <summary>
        /// Returns a paginated list of <see cref="Sale"/> entities, ordered by <c>Date</c>.
        /// </summary>
        /// <param name="page">The 1‐based page number.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        /// <returns>A read‐only list of <see cref="Sale"/> entities for the requested page.</returns>
        public async Task<IReadOnlyList<Sale>> ListAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
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
        /// Deletes all child <see cref="SaleItem"/> rows for a given <see cref="Sale"/> ID using raw SQL.
        /// </summary>
        /// <param name="saleId">The parent <see cref="Sale"/> identifier.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task DeleteAllItemsBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                DELETE FROM ""SaleItems""
                WHERE ""SaleId"" = {0};";

            // Pass the saleId as a SQL parameter; cancellationToken is applied separately.
            await _context.Database.ExecuteSqlRawAsync(
                sql,
                new object[] { saleId },
                cancellationToken
            );
        }

        /// <summary>
        /// Inserts a collection of new <see cref="SaleItem"/> rows for the specified <see cref="Sale"/> ID
        /// using raw SQL. Assumes each <see cref="SaleItem"/> in <paramref name="items"/> has a valid GUID
        /// and pre‐computed <c>DiscountRate</c>.
        /// </summary>
        /// <param name="items">The list of <see cref="SaleItem"/> entities to insert.</param>
        /// <param name="saleId">The parent <see cref="Sale"/> identifier (foreign key).</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for async operation.</param>
        public async Task AddItemsAsync(IEnumerable<SaleItem> items, Guid saleId, CancellationToken cancellationToken = default)
        {
            const string insertSql = @"
                INSERT INTO ""SaleItems"" (
                    ""Id"",
                    ""ProductId"",
                    ""Quantity"",
                    ""UnitPrice"",
                    ""DiscountRate"",
                    ""IsCancelled"",
                    ""SaleId""
                ) VALUES (
                    {0},  -- SaleItem.Id
                    {1},  -- ProductId
                    {2},  -- Quantity
                    {3},  -- UnitPrice
                    {4},  -- DiscountRate
                    FALSE,
                    {5}   -- SaleId (foreign key)
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
                        saleId
                    },
                    cancellationToken
                );
            }
        }
    }
}
