using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Validates the <see cref="CancelSaleCommand"/> to ensure it contains a non-empty sale identifier.
    /// </summary>
    public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelSaleValidator"/> class
        /// and defines rules for <see cref="CancelSaleCommand"/>.
        /// </summary>
        public CancelSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale Id is required.");
        }
    }
}

