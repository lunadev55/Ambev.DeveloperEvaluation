using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Validates the <see cref="UpdateSaleCommand"/> to ensure that all required fields
    /// are provided and conform to business constraints when updating a sale.
    /// </summary>
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleValidator"/> class,
        /// defining validation rules for <see cref="UpdateSaleCommand"/>.
        /// </summary>
        public UpdateSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale Id is required.");

            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .WithMessage("SaleNumber is required.");

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
                .SetValidator(new SaleItemDtoValidator());
        }
    }

    /// <summary>
    /// Validates each <see cref="UpdateSaleItemDto"/> within an update command,
    /// ensuring that product identifiers, quantities, and pricing are valid.
    /// </summary>
    internal class SaleItemDtoValidator : AbstractValidator<UpdateSaleItemDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleItemDtoValidator"/> class,
        /// defining validation rules for <see cref="UpdateSaleItemDto"/>.
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
