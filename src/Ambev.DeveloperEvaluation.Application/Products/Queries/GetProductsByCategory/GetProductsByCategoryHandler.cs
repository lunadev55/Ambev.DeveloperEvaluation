using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.ListAsync(request.Page, request.Size, request.OrderBy, cancellationToken);
            var filtered = products.Where(p => p.Category == request.Category).ToList();
            var items = _mapper.Map<IEnumerable<ProductListItem>>(filtered);
            var totalItems = filtered.Count;
            var totalPages = (int)System.Math.Ceiling((double)totalItems / request.Size);

            return new GetProductsByCategoryResult
            {
                Data = items,
                CurrentPage = request.Page,
                TotalPages = totalPages,
                TotalItems = totalItems
            };
        }
    }
}
