using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    /// <summary>
    /// Validates the <see cref="GetSalesListQuery"/>, ensuring that pagination parameters are positive.
    /// </summary>
    public class GetSalesListValidator : AbstractValidator<GetSalesListQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSalesListValidator"/> class
        /// and defines rules for <see cref="GetSalesListQuery"/>.
        /// </summary>
        public GetSalesListValidator()
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
