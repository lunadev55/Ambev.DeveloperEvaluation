using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Unit tests for the <see cref="UpdateCartHandler"/> class.
    /// </summary>
    public class UpdateCartHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly UpdateCartHandler _handler;

        public UpdateCartHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _productRepository = Substitute.For<IProductRepository>();

            // A no-op logger is sufficient for testing
            var logger = Substitute.For<ILogger<UpdateCartHandler>>();

            _handler = new UpdateCartHandler(
                _cartRepository,
                _userRepository,
                _productRepository,
                logger
            );
        }

        [Fact(DisplayName = "Given valid command When handling Then calls repository.DeleteAllItems, AddItems, Update, and SaveChanges")]
        public async Task Handle_ValidCommand_CallsExpectedRepositoryMethods()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var branchValue = "TestBranch";
            var initialCartNumber = "C-100";
            var initialDate = DateTime.UtcNow.Date.AddDays(-1);

            // 1) Create an existing Cart with the correct signature
            var existingCart = new Cart(
                id: cartId,
                cartNumber: initialCartNumber,
                date: initialDate,
                customerId: new CustomerId(customerId),
                branch: branchValue
            );

            _cartRepository
                .GetByIdAsync(cartId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(existingCart));

            // 2) Create a User (parameterless) and set its properties
            var user = new User();
            // BaseEntity.Id is inherited, so we assign via reflection (Id has a protected setter).
            typeof(User).GetProperty(nameof(User.Id))
                ?.SetValue(user, customerId);
            user.Role = UserRole.Customer;
            user.Status = UserStatus.Active;
            _userRepository
                .GetByIdAsync(customerId)
                .Returns(Task.FromResult(user));

            // 3) Create two products
            var product1 = new Product(
                id: Guid.NewGuid(),
                title: "Product A",
                price: 10m,
                description: "Desc A",
                category: "CatA",
                image: "imgA.jpg",
                rating: new Rating(4.5m, 100)
            );
            var product2 = new Product(
                id: Guid.NewGuid(),
                title: "Product B",
                price: 20m,
                description: "Desc B",
                category: "CatB",
                image: "imgB.jpg",
                rating: new Rating(3.0m, 50)
            );

            _productRepository
                .GetByIdAsync(product1.Id)
                .Returns(Task.FromResult(product1));
            _productRepository
                .GetByIdAsync(product2.Id)
                .Returns(Task.FromResult(product2));

            // 4) Build the UpdateCartCommand with two item DTOs
            var command = new UpdateCartCommand
            {
                Id = cartId,
                CartNumber = "C-200",
                Date = initialDate.AddDays(1),
                CustomerId = customerId,
                Branch = branchValue,
                Items = new List<UpdateCartItemDto>
                {
                    new UpdateCartItemDto
                    {
                        ProductId = product1.Id,
                        Quantity  = 2,
                        UnitPrice = 10m
                    },
                    new UpdateCartItemDto
                    {
                        ProductId = product2.Id,
                        Quantity  = 5,
                        UnitPrice = 20m
                    }
                }
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();

            // DeleteAllItemsByCartIdAsync must have been called exactly once with cartId
            await _cartRepository.Received(1)
                .DeleteAllItemsByCartIdAsync(cartId, Arg.Any<CancellationToken>());

            // AddItemsAsync must have been called with exactly two new CartItem objects
            //await _cartRepository.Received(1)
            //    .AddItemsAsync(
            //        Arg.Is<IEnumerable<CartItem>>(items =>
            //        {
            //            var list = new List<CartItem>(items);
            //            return list.Count == 2
            //                   && list[0].ProductId == product1.Id
            //                   && list[1].ProductId == product2.Id;
            //        }),
            //        cartId,
            //        Arg.Any<CancellationToken>()
            //    );
            await _cartRepository.Received(1)
                .AddItemsAsync(
                    Arg.Is<IEnumerable<CartItem>>(items =>
                        items.Count() == 2
                        && items.Any(i => i.ProductId == product1.Id)
                        && items.Any(i => i.ProductId == product2.Id)
                    ),
                    cartId,
                    Arg.Any<CancellationToken>()
                );


            // Update(cart) must have been called, with the cart header updated
            _cartRepository.Received(1)
                .Update(Arg.Is<Cart>(c =>
                    c.Id == cartId &&
                    c.CustomerId.Value == customerId &&
                    c.Branch == branchValue &&
                    c.CartNumber == "C-200"
                ));

            // Finally, SaveChangesAsync must have been called exactly once
            await _cartRepository.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given non‐existent cart When handling Then throws KeyNotFoundException")]
        public async Task Handle_CartNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var missingCartId = Guid.NewGuid();
            _cartRepository
                .GetByIdAsync(missingCartId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<Cart>(null));

            var cmd = new UpdateCartCommand
            {
                Id = missingCartId,
                CartNumber = "C-999",
                Date = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                Branch = "NoBranch",
                Items = new List<UpdateCartItemDto>()
            };

            // Act
            Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"Cart with ID '{missingCartId}' not found.");

            // Verify that no other repository methods were called
            await _cartRepository.DidNotReceiveWithAnyArgs().DeleteAllItemsByCartIdAsync(default, default);
            await _cartRepository.DidNotReceiveWithAnyArgs().AddItemsAsync(default, default, default);
            _cartRepository.DidNotReceiveWithAnyArgs().Update(default);
            await _cartRepository.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
        }

        [Fact(DisplayName = "Given missing or non‐Customer user When handling Then throws KeyNotFoundException")]
        public async Task Handle_UserNotCustomer_ThrowsKeyNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var callerUserId = Guid.NewGuid();
            var existingCart = new Cart(
                id: cartId,
                cartNumber: "C-101",
                date: DateTime.UtcNow.Date,
                customerId: new CustomerId(callerUserId),
                branch: "BranchX"
            );
            _cartRepository
                .GetByIdAsync(cartId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(existingCart));

            // Case A: user not found
            _userRepository.GetByIdAsync(callerUserId).Returns(Task.FromResult<User>(null));

            var cmdA = new UpdateCartCommand
            {
                Id = cartId,
                CartNumber = "C-101",
                Date = DateTime.UtcNow,
                CustomerId = callerUserId,
                Branch = "BranchX",
                Items = new List<UpdateCartItemDto>()
            };

            Func<Task> actA = () => _handler.Handle(cmdA, CancellationToken.None);
            await actA.Should().ThrowAsync<KeyNotFoundException>()
                      .WithMessage($"Customer with '{callerUserId}' not found.");

            // Case B: user found but not a Customer
            var nonCustomerUser = new User();
            typeof(User).GetProperty(nameof(User.Id))
                ?.SetValue(nonCustomerUser, callerUserId);
            nonCustomerUser.Role = UserRole.Manager; // Not a Customer

            _userRepository
                .GetByIdAsync(callerUserId)
                .Returns(Task.FromResult(nonCustomerUser));

            var cmdB = new UpdateCartCommand
            {
                Id = cartId,
                CartNumber = "C-102",
                Date = DateTime.UtcNow,
                CustomerId = callerUserId,
                Branch = "BranchY",
                Items = new List<UpdateCartItemDto>()
            };

            Func<Task> actB = () => _handler.Handle(cmdB, CancellationToken.None);
            await actB.Should().ThrowAsync<KeyNotFoundException>()
                      .WithMessage($"Customer with '{callerUserId}' not found.");

            // Verify repository methods are never called beyond GetByIdAsync
            await _cartRepository.DidNotReceiveWithAnyArgs().DeleteAllItemsByCartIdAsync(default, default);
            await _cartRepository.DidNotReceiveWithAnyArgs().AddItemsAsync(default, default, default);
            _cartRepository.DidNotReceiveWithAnyArgs().Update(default);
            await _cartRepository.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
        }

        [Fact(DisplayName = "Given missing product When handling Then throws KeyNotFoundException")]
        public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var callerUserId = Guid.NewGuid();
            var existingCart = new Cart(
                id: cartId,
                cartNumber: "C-300",
                date: DateTime.UtcNow.Date,
                customerId: new CustomerId(callerUserId),
                branch: "BranchZ"
            );

            _cartRepository
                .GetByIdAsync(cartId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(existingCart));

            // User exists and is a Customer
            var customer = new User();
            typeof(User).GetProperty(nameof(User.Id))
                ?.SetValue(customer, callerUserId);
            customer.Role = UserRole.Customer;
            _userRepository
                .GetByIdAsync(callerUserId)
                .Returns(Task.FromResult(customer));

            // Only one product exists; the other is missing
            var existingProduct = new Product(
                id: Guid.NewGuid(),
                title: "ValidProd",
                price: 5m,
                description: "ValidDesc",
                category: "CatX",
                image: "imgX.jpg",
                rating: new Rating(4.0m, 10)
            );
            _productRepository.GetByIdAsync(existingProduct.Id)
                               .Returns(Task.FromResult(existingProduct));

            var missingProductId = Guid.NewGuid();
            _productRepository.GetByIdAsync(missingProductId)
                               .Returns(Task.FromResult<Product>(null));

            var cmd = new UpdateCartCommand
            {
                Id = cartId,
                CartNumber = "C-300",
                Date = DateTime.UtcNow,
                CustomerId = callerUserId,
                Branch = "BranchZ",
                Items = new List<UpdateCartItemDto>
                {
                    new UpdateCartItemDto
                    {
                        ProductId = existingProduct.Id,
                        Quantity  = 1,
                        UnitPrice = 5m
                    },
                    new UpdateCartItemDto
                    {
                        ProductId = missingProductId,
                        Quantity  = 2,
                        UnitPrice = 7.5m
                    }
                }
            };

            // Act
            Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"Product with Id {missingProductId} not found.");

            // Verify that DeleteAllItems and AddItems were never invoked
            await _cartRepository.DidNotReceiveWithAnyArgs().DeleteAllItemsByCartIdAsync(default, default);
            await _cartRepository.DidNotReceiveWithAnyArgs().AddItemsAsync(default, default, default);
            _cartRepository.DidNotReceiveWithAnyArgs().Update(default);
            await _cartRepository.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
        }
    }
}
