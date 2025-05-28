using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _repository;

        public CancelSaleHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID '{request.Id}' not found.");

            sale.CancelSale();
            _repository.Update(sale);
            await _repository.SaveChangesAsync(cancellationToken);

            return new CancelSaleResult { Success = true };
        }
    }
}
