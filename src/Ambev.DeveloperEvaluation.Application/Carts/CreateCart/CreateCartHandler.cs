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
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCartHandler"/> class.
        /// </summary>
        /// <param name="cartRepository">
        /// The repository for persisting <see cref="Cart"/> aggregates.
        /// </param>
        /// <param name="userRepository">
        /// The repository for accessing <see cref="User"/> entities to validate the customer role.
        /// </param>
        /// /// <param name="productRepository">
        /// The repository for accessing <see cref="Product"/> entities.
        /// </param>
        public CreateCartHandler(
            ICartRepository cartRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
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
                throw new KeyNotFoundException($"Customer '{request.CustomerId}' not found");
                        
            var cart = new Cart(
                Guid.NewGuid(),
                request.CartNumber,
                request.Date,                
                new CustomerId(request.CustomerId),
                request.Branch
                );
                    
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product Id {item.ProductId} not found");
                cart.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
            }
                        
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync(cancellationToken);
                        
            return new CreateCartResult { Id = cart.Id };
        }
    }
}

