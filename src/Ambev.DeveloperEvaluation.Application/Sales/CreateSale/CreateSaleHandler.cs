using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {        
        private readonly ISaleRepository _repository;

        public CreateSaleHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {            
            var sale = new Sale(
                Guid.NewGuid(),
                request.SaleNumber,
                request.Date,
                new CustomerId(request.CustomerId),
                new BranchId(request.BranchId));
                        
            foreach (var dto in request.Items)
            {
                sale.AddItem(dto.ProductId, dto.Quantity, dto.UnitPrice);
            }
                        
            await _repository.AddAsync(sale);
            await _repository.SaveChangesAsync(cancellationToken);

            return new CreateSaleResult { Id = sale.Id };
        }
    }
}
