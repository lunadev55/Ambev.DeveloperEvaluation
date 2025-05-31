using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResult>
    {
        private readonly IProductRepository _repository;

        public GetCategoriesHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetCategoriesResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.ListAsync(1, int.MaxValue, null, cancellationToken);
            var categories = products.Select(p => p.Category).Distinct();
            return new GetCategoriesResult { Data = categories };
        }
    }
}
