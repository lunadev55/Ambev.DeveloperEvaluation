using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _repository;

        public UpdateSaleHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID '{request.Id}' not found.");

            // TODO: Implement domain methods to update sale header and items
            // e.g., sale.UpdateSaleNumber(request.SaleNumber);
            //       sale.UpdateDate(request.Date);
            //       sale.UpdateCustomer(request.CustomerId, request.BranchId);
            //       sale.ReplaceItems(request.Items.Select(dto => new SaleItem(...)));

            _repository.Update(sale);
            await _repository.SaveChangesAsync(cancellationToken);

            return new UpdateSaleResult { Success = true };
        }
    }
}
