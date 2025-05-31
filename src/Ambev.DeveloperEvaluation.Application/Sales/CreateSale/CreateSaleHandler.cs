using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {        
        private readonly ISaleRepository _repository;
        private readonly IUserRepository _userRepository;

        public CreateSaleHandler(
            ISaleRepository repository, 
            IUserRepository userRepo)
        {
            _repository = repository;
            _userRepository = userRepo;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.CustomerId);
            if (user == null || user.Role != UserRole.Customer)
                throw new DomainException("Customer not found");

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
