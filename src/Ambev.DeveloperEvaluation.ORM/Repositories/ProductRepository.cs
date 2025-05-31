using Ambev.DeveloperEvaluation.Common.Extensions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="IProductRepository"/> using DefaultContext.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;

        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> ListAsync(
            int page,
            int size,
            string orderBy = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var orders = orderBy.Split(',');
                foreach (var ord in orders)
                {
                    var parts = ord.Trim().Split(' ');
                    var prop = parts[0];
                    var desc = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);
                    //query = query.OrderByProperty(prop, desc);
                    query = QueryableExtensions
                        .OrderByProperty(query, prop, desc);

                }
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            _context.Products.Add(product);
            return await Task.FromResult(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
