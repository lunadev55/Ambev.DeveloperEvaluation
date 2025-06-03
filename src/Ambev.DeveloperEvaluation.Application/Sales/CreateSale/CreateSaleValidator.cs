using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validates the <see cref="CreateSaleCommand"/> to ensure all required fields
    /// and constraints for creating a sale are met.
    /// </summary>
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleValidator"/> class
        /// and defines validation rules for <see cref="CreateSaleCommand"/>.
        /// </summary>
        public CreateSaleValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .WithMessage("SaleNumber is required.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("BranchId is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new SaleItemDtoValidator());
        }
    }

    /// <summary>
    /// Validates each <see cref="CreateSaleItemDto"/> to ensure product, quantity,
    /// and pricing constraints are within allowed ranges.
    /// </summary>
    internal class SaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleItemDtoValidator"/> class
        /// and defines validation rules for <see cref="CreateSaleItemDto"/>.
        /// </summary>
        public SaleItemDtoValidator()
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

