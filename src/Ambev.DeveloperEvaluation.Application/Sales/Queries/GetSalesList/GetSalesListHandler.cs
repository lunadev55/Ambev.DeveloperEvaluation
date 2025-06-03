using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    /// <summary>
    /// Handles the retrieval of a paginated list of sales, mapping domain entities into DTOs.
    /// </summary>
    public class GetSalesListHandler : IRequestHandler<GetSalesListQuery, GetSalesListResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSalesListHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to fetch <see cref="Domain.Entities.Sale"/> aggregates.
        /// </param>
        /// <param name="mapper">
        /// The AutoMapper instance used to project domain entities to <see cref="SalesListItem"/> DTOs.
        /// </param>
        public GetSalesListHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes the <see cref="GetSalesListQuery"/>, retrieving the specified page of sales,
        /// mapping each <see cref="Domain.Entities.Sale"/> to a <see cref="SalesListItem"/>, and
        /// returning the paginated result.
        /// </summary>
        /// <param name="request">
        /// The query containing pagination parameters: page number and page size.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while awaiting the database operation.
        /// </param>
        /// <returns>
        /// A <see cref="GetSalesListResult"/> containing the requested page, page size, and mapped items.
        /// </returns>
        public async Task<GetSalesListResult> Handle(GetSalesListQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the paginated domain sales from the repository
            var sales = await _repository.ListAsync(request.Page, request.Size, cancellationToken);

            // Map domain entities to list‐item DTOs
            var items = _mapper.Map<List<SalesListItem>>(sales);

            // Return the paginated result
            return new GetSalesListResult
            {
                Page = request.Page,
                Size = request.Size,
                Items = items
            };
        }
    }
}

