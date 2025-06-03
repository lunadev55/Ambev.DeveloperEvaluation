using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    /// <summary>
    /// Validates the <see cref="GetSaleByIdQuery"/> to ensure it contains a non-empty sale identifier.
    /// </summary>
    public class GetSaleByIdValidator : AbstractValidator<GetSaleByIdQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleByIdValidator"/> class
        /// and defines the rule that the sale ID must not be empty.
        /// </summary>
        public GetSaleByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale Id is required.");
        }
    }
}

