using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Handles the creation of a new <see cref="Cart"/>.
    /// Validates the customer, constructs the cart aggregate, adds items, and persists it.
    /// </summary>
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
    {
        private readonly ICartRepository _repository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCartHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository for persisting <see cref="Cart"/> aggregates.
        /// </param>
        /// <param name="userRepo">
        /// The repository for accessing <see cref="User"/> entities to validate the customer role.
        /// </param>
        public CreateCartHandler(
            ICartRepository repository,
            IUserRepository userRepo)
        {
            _repository = repository;
            _userRepository = userRepo;
        }

        /// <summary>
        /// Processes a <see cref="CreateCartCommand"/> by verifying the customer’s role,
        /// creating a new <see cref="Cart"/> with its items, and saving it to the database.
        /// </summary>
        /// <param name="request">
        /// The <see cref="CreateCartCommand"/> containing cart header details and item DTOs.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while awaiting asynchronous operations.
        /// </param>
        /// <returns>
        /// A <see cref="CreateCartResult"/> containing the newly created cart’s ID.
        /// </returns>
        /// <exception cref="DomainException">
        /// Thrown if the specified customer does not exist or is not in the <see cref="UserRole.Customer"/> role.
        /// </exception>
        public async Task<CreateCartResult> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {            
            var user = await _userRepository.GetByIdAsync(request.CustomerId);
            if (user == null || user.Role != UserRole.Customer)
                throw new DomainException("Customer not found");
                        
            var cart = new Cart(
                Guid.NewGuid(),
                request.CartNumber,
                request.Date,                
                new CustomerId(request.CustomerId),
                request.Branch
                );
                    
            foreach (var dto in request.Items)
            {
                cart.AddItem(dto.ProductId, dto.Quantity, dto.UnitPrice);
            }
                        
            await _repository.AddAsync(cart);
            await _repository.SaveChangesAsync(cancellationToken);
                        
            return new CreateCartResult { Id = cart.Id };
        }
    }
}

