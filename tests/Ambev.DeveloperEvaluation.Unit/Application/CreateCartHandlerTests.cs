using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
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
    /// Unit tests for the <see cref="CreateCartHandler"/> class.
    /// </summary>
    public class CreateCartHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly CreateCartHandler _handler;

        public CreateCartHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _productRepository = Substitute.For<IProductRepository>();

            // No-op logger
            var logger = Substitute.For<ILogger<CreateCartHandler>>();

            _handler = new CreateCartHandler(
                _cartRepository,
                _userRepository,
                _productRepository,
                logger
            );
        }

        [Fact(DisplayName = "Given valid command When handling Then adds cart and returns Id")]
        public async Task Handle_ValidCommand_AddsCartAndReturnsId()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var branch = "BranchA";

            // Stub user as a valid Customer
            var user = new User();
            typeof(User).GetProperty(nameof(User.Id))?.SetValue(user, customerId);
            user.Role = UserRole.Customer;
            _userRepository
                .GetByIdAsync(customerId)
                .Returns(Task.FromResult(user));

            // Create two products
            var prod1 = new Product(
                id: Guid.NewGuid(),
                title: "Prod1",
                price: 10m,
                description: "Desc1",
                category: "Cat1",
                image: "img1.png",
                rating: new Rating(4.2m, 20)
            );
            var prod2 = new Product(
                id: Guid.NewGuid(),
                title: "Prod2",
                price: 5m,
                description: "Desc2",
                category: "Cat2",
                image: "img2.png",
                rating: new Rating(3.8m, 8)
            );

            _productRepository
                .GetByIdAsync(prod1.Id)
                .Returns(Task.FromResult(prod1));
            _productRepository
                .GetByIdAsync(prod2.Id)
                .Returns(Task.FromResult(prod2));

            // Build command with two items
            var command = new CreateCartCommand
            {
                CartNumber = "CART-123",
                Date = DateTime.UtcNow.Date,
                CustomerId = customerId,
                Branch = branch,
                Items = new List<CreateCartItemDto>
                {
                    new CreateCartItemDto
                    {
                        ProductId = prod1.Id,
                        Quantity  = 3,
                        UnitPrice = 10m
                    },
                    new CreateCartItemDto
                    {
                        ProductId = prod2.Id,
                        Quantity  = 5,
                        UnitPrice = 5m
                    }
                }
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Id.Should().NotBe(Guid.Empty);

            // Verify AddAsync was called with a Cart whose properties match the command
            await _cartRepository.Received(1).AddAsync(
                Arg.Is<Cart>(cart =>
                    cart.CartNumber == command.CartNumber &&
                    cart.CustomerId.Value == customerId &&
                    cart.Branch == branch &&
                    cart.Items.Count == command.Items.Count
                ),
                Arg.Any<CancellationToken>());

            await _cartRepository.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given missing user When handling Then throws KeyNotFoundException")]
        public async Task Handle_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var missingCustomerId = Guid.NewGuid();

            _userRepository
                .GetByIdAsync(missingCustomerId)
                .Returns(Task.FromResult<User>(null));

            var cmd = new CreateCartCommand
            {
                CartNumber = "CART-404",
                Date = DateTime.UtcNow,
                CustomerId = missingCustomerId,
                Branch = "NoBranch",
                Items = new List<CreateCartItemDto>()
            };

            // Act
            Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"Customer '{missingCustomerId}' not found");

            await _cartRepository.DidNotReceiveWithAnyArgs().AddAsync(default, default);
            await _cartRepository.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
        }

        [Fact(DisplayName = "Given user not in Customer role When handling Then throws KeyNotFoundException")]
        public async Task Handle_UserNotCustomerRole_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User();
            typeof(User).GetProperty(nameof(User.Id))?.SetValue(user, userId);
            user.Role = UserRole.Manager; // Not a Customer

            _userRepository
                .GetByIdAsync(userId)
                .Returns(Task.FromResult(user));

            var cmd = new CreateCartCommand
            {
                CartNumber = "CART-505",
                Date = DateTime.UtcNow,
                CustomerId = userId,
                Branch = "BranchX",
                Items = new List<CreateCartItemDto>()
            };

            // Act
            Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"Customer '{userId}' not found");

            await _cartRepository.DidNotReceiveWithAnyArgs().AddAsync(default, default);
            await _cartRepository.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
        }

        [Fact(DisplayName = "Given missing product When handling Then throws KeyNotFoundException")]
        public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var user = new User();
            typeof(User).GetProperty(nameof(User.Id))?.SetValue(user, customerId);
            user.Role = UserRole.Customer;

            _userRepository.GetByIdAsync(customerId)
                           .Returns(Task.FromResult(user));

            // One valid product, one missing
            var validProd = new Product(
                id: Guid.NewGuid(),
                title: "Valid",
                price: 7m,
                description: "ValidDesc",
                category: "CatY",
                image: "imgY.png",
                rating: new Rating(4.0m, 12)
            );
            _productRepository
                .GetByIdAsync(validProd.Id)
                .Returns(Task.FromResult(validProd));

            var missingProdId = Guid.NewGuid();
            _productRepository
                .GetByIdAsync(missingProdId)
                .Returns(Task.FromResult<Product>(null));

            var cmd = new CreateCartCommand
            {
                CartNumber = "CART-310",
                Date = DateTime.UtcNow,
                CustomerId = customerId,
                Branch = "BranchZ",
                Items = new List<CreateCartItemDto>
                {
                    new CreateCartItemDto
                    {
                        ProductId = validProd.Id,
                        Quantity  = 2,
                        UnitPrice = 7m
                    },
                    new CreateCartItemDto
                    {
                        ProductId = missingProdId,
                        Quantity  = 1,
                        UnitPrice = 5m
                    }
                }
            };

            // Act
            Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"Product Id {missingProdId} not found");

            await _cartRepository.DidNotReceiveWithAnyArgs().AddAsync(default, default);
            await _cartRepository.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
        }
    }
}
