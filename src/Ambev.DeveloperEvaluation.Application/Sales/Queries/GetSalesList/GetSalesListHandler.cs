using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    public class GetSalesListHandler : IRequestHandler<GetSalesListQuery, GetSalesListResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public GetSalesListHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetSalesListResult> Handle(GetSalesListQuery request, CancellationToken cancellationToken)
        {
            var sales = await _repository.ListAsync(request.Page, request.Size, cancellationToken);
            var items = _mapper.Map<List<SalesListItem>>(sales);

            return new GetSalesListResult
            {
                Page = request.Page,
                Size = request.Size,
                Items = items
            };
        }
    }
}
