using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;
        public SaleRepository(DefaultContext context) => _context = context;

        public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Add(sale);
            await Task.CompletedTask;
        }

        public void Update(Sale sale)
        {
            _context.Sales.Update(sale);
        }

        public async Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Sale>> ListAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .OrderBy(s => s.Date)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
