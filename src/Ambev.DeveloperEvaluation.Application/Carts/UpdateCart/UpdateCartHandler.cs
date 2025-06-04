using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Handles the <see cref="UpdateCartCommand"/> by replacing all existing items for a cart
    /// and updating the cart header (number, date, customer, branch).
    /// </summary>
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCartHandler"/> class.
        /// </summary>
        /// <param name="cartRepository">
        /// The <see cref="ICartRepository"/> used to load, delete, and save cart and cart-item data.
        /// </param>
        /// /// <param name="userRepository">
        /// The <see cref="IUserRepository"/> used to load user data.
        /// </param>
        /// /// <param name="productRepository">
        /// The <see cref="IProductRepository"/> used to load product data.
        /// </param>
        public UpdateCartHandler(
            ICartRepository cartRepository,
            IUserRepository userRepository,
            IProductRepository productRepository    )
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Processes the update request by:
        /// 1. Loading the existing <see cref="Cart"/> aggregate (including its items).
        /// 2. Updating header fields (cart number, date, customer, branch).
        /// 3. Deleting all currently stored <see cref="CartItem"/> rows for that cart.
        /// 4. Inserting new <see cref="CartItem"/> rows based on the incoming DTOs.
        /// 5. Marking the cart header as modified and persisting any changes.
        /// </summary>
        /// <param name="request">
        /// The <see cref="UpdateCartCommand"/> containing the cart ID, new header values, and item DTOs.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe for cancellation requests.
        /// </param>
        /// <returns>
        /// An <see cref="UpdateCartResult"/> indicating success or failure.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no cart with the specified ID exists.
        /// </exception>
        public async Task<UpdateCartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {            
            var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID '{request.Id}' not found.");

            var user = await _userRepository.GetByIdAsync(request.CustomerId);
            if (user == null || user.Role != UserRole.Customer)
                throw new KeyNotFoundException($"Customer with '{request.CustomerId}' not found.");

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with Id {item.ProductId} not found.");                
            }

            cart.UpdateCartNumber(request.CartNumber);
            cart.UpdateDate(request.Date);
            cart.UpdateCustomer(new CustomerId(request.CustomerId));
            cart.UpdateBranch(request.Branch);
            
            var newItems = request.Items
                .Select(dto =>
                {
                    var discountPct = cart.CalculateDiscount(dto.Quantity);
                    return new CartItem(
                        Guid.NewGuid(),
                        dto.ProductId,
                        dto.Quantity,
                        dto.UnitPrice,
                        discountPct
                    );
                })
                .ToList();
            
            await _cartRepository.DeleteAllItemsByCartIdAsync(cart.Id, cancellationToken);
            
            await _cartRepository.AddItemsAsync(newItems, cart.Id, cancellationToken);

            _cartRepository.Update(cart);
            
            await _cartRepository.SaveChangesAsync(cancellationToken);

            return new UpdateCartResult { Success = true };
        }
    }
}
