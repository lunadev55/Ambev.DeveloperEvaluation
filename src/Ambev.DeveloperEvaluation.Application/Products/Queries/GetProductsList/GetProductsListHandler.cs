using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList
{
    public class GetProductsListHandler : IRequestHandler<GetProductsListQuery, GetProductsListResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsListHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetProductsListResult> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.ListAsync(request.Page, request.Size, request.OrderBy, cancellationToken);
            var items = _mapper.Map<IEnumerable<ProductListItem>>(products);
            var totalItems = products.Count();
            var totalPages = (int)System.Math.Ceiling((double)totalItems / request.Size);

            return new GetProductsListResult
            {
                Data = items,
                CurrentPage = request.Page,
                TotalPages = totalPages,
                TotalItems = totalItems
            };
        }
    }
}
