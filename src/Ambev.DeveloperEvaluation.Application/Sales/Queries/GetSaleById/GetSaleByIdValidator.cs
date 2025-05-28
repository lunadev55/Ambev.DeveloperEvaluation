using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdValidator : AbstractValidator<GetSaleByIdQuery>
    {
        public GetSaleByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Sale Id is required.");
        }
    }
}
