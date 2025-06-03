using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handles the creation of a new <see cref="Sale"/>.
    /// Validates the customer, constructs the sale aggregate, adds items, and persists it.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository for persisting <see cref="Sale"/> aggregates.
        /// </param>
        /// <param name="userRepo">
        /// The repository for accessing <see cref="User"/> entities to validate the customer role.
        /// </param>
        public CreateSaleHandler(
            ISaleRepository repository,
            IUserRepository userRepo)
        {
            _repository = repository;
            _userRepository = userRepo;
        }

        /// <summary>
        /// Processes a <see cref="CreateSaleCommand"/> by verifying the customer’s role,
        /// creating a new <see cref="Sale"/> with its items, and saving it to the database.
        /// </summary>
        /// <param name="request">
        /// The <see cref="CreateSaleCommand"/> containing sale header details and item DTOs.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while awaiting asynchronous operations.
        /// </param>
        /// <returns>
        /// A <see cref="CreateSaleResult"/> containing the newly created sale’s ID.
        /// </returns>
        /// <exception cref="DomainException">
        /// Thrown if the specified customer does not exist or is not in the <see cref="UserRole.Customer"/> role.
        /// </exception>
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
                request.Branch
                );
                    
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

