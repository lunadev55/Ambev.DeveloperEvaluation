using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product '{request.Id}' not found.");

            product.Update(
                request.Title,
                request.Price,
                request.Description,
                request.Category,
                request.Image,
                new Rating(request.RatingRate, request.RatingCount)
            );

            _repository.Update(product);
            await _repository.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult { Success = true };
        }
    }
}
