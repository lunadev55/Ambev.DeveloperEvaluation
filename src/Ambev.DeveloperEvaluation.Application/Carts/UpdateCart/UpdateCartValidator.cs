using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Validates the <see cref="UpdateCartCommand"/> to ensure that all required fields
    /// are provided and conform to business constraints when updating a cart.
    /// </summary>
    public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCartValidator"/> class,
        /// defining validation rules for <see cref="UpdateCartCommand"/>.
        /// </summary>
        public UpdateCartValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Cart   Id is required.");

            RuleFor(x => x.CartNumber)
                .NotEmpty()
                .WithMessage("CartNumber is required.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.Branch)
                .NotEmpty()
                .WithMessage("Branch is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new CartItemDtoValidator());
        }
    }

    /// <summary>
    /// Validates each <see cref="UpdateCartItemDto"/> within an update command,
    /// ensuring that product identifiers, quantities, and pricing are valid.
    /// </summary>
    internal class CartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemDtoValidator"/> class,
        /// defining validation rules for <see cref="UpdateCartItemDto"/>.
        /// </summary>
        public CartItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Quantity)
                .LessThanOrEqualTo(20)
                .WithMessage("Quantity must not exceed 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("UnitPrice must be greater than zero.");
        }
    }
}
