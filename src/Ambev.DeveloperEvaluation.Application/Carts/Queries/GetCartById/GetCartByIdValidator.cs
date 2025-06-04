using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById
{
    /// <summary>
    /// Validates the <see cref="GetCartByIdQuery"/> to ensure it contains a non-empty cart identifier.
    /// </summary>
    public class GetCartByIdValidator : AbstractValidator<GetCartByIdQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartByIdValidator"/> class
        /// and defines the rule that the cart ID must not be empty.
        /// </summary>
        public GetCartByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Cart Id is required.");
        }
    }
}

