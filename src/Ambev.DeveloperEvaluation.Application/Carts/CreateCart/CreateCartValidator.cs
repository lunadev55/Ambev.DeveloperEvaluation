using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Validates the <see cref="CreateCartCommand"/> to ensure all required fields
    /// and constraints for creating a cart are met.
    /// </summary>
    public class CreateCartValidator : AbstractValidator<CreateCartCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCartValidator"/> class
        /// and defines validation rules for <see cref="CreateCartCommand"/>.
        /// </summary>
        public CreateCartValidator()
        {
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
    /// Validates each <see cref="CreateCartItemDto"/> to ensure product, quantity,
    /// and pricing constraints are within allowed ranges.
    /// </summary>
    internal class CartItemDtoValidator : AbstractValidator<CreateCartItemDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemDtoValidator"/> class
        /// and defines validation rules for <see cref="CreateCartItemDto"/>.
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

