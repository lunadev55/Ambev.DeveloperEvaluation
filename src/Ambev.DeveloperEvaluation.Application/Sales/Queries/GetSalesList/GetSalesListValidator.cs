using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    public class GetSalesListValidator : AbstractValidator<GetSalesListQuery>
    {
        public GetSalesListValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than zero.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Size must be greater than zero.");
        }
    }
}
