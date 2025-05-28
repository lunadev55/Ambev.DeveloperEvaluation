using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Sale Id is required.");

            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("SaleNumber is required.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId is required.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.");

            RuleForEach(x => x.Items).SetValidator(new SaleItemDtoValidator());
        }
    }

    internal class SaleItemDtoValidator : AbstractValidator<SaleItemDto>
    {
        public SaleItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Quantity)
                .LessThanOrEqualTo(20).WithMessage("Quantity must not exceed 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");
        }
    }
}
