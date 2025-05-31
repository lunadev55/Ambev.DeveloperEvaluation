using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image URL is required");
            RuleFor(x => x.RatingRate).InclusiveBetween(0m, 5m);
            RuleFor(x => x.RatingCount).GreaterThanOrEqualTo(0);
        }
    }
}
