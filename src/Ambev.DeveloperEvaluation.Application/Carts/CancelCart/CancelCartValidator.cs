using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart
{
    /// <summary>
    /// Validates the <see cref="CancelCartCommand"/> to ensure it contains a non-empty cart identifier.
    /// </summary>
    public class CancelCartValidator : AbstractValidator<CancelCartCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelCartValidator"/> class
        /// and defines rules for <see cref="CancelCartCommand"/>.
        /// </summary>
        public CancelCartValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Cart Id is required.");
        }
    }
}

