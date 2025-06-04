using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartsList
{
    /// <summary>
    /// Validates the <see cref="GetCartsListQuery"/>, ensuring that pagination parameters are positive.
    /// </summary>
    public class GetCartsListValidator : AbstractValidator<GetCartsListQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartsListValidator"/> class
        /// and defines rules for <see cref="GetCartsListQuery"/>.
        /// </summary>
        public GetCartsListValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than zero.");

            RuleFor(x => x.Size)
                .GreaterThan(0)
                .WithMessage("Size must be greater than zero.");
        }
    }
}
