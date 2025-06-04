using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartsList
{
    /// <summary>
    /// Handles the retrieval of a paginated list of carts, mapping domain entities into DTOs.
    /// </summary>
    public class GetCartsListHandler : IRequestHandler<GetCartsListQuery, GetCartsListResult>
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartsListHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to fetch <see cref="Domain.Entities.Cart"/> aggregates.
        /// </param>
        /// <param name="mapper">
        /// The AutoMapper instance used to project domain entities to <see cref="CartsListItem"/> DTOs.
        /// </param>
        public GetCartsListHandler(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes the <see cref="GetCartsListQuery"/>, retrieving the specified page of carts,
        /// mapping each <see cref="Domain.Entities.Cart"/> to a <see cref="CartsListItem"/>, and
        /// returning the paginated result.
        /// </summary>
        /// <param name="request">
        /// The query containing pagination parameters: page number and page size.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while awaiting the database operation.
        /// </param>
        /// <returns>
        /// A <see cref="GetCartsListResult"/> containing the requested page, page size, and mapped items.
        /// </returns>
        public async Task<GetCartsListResult> Handle(GetCartsListQuery request, CancellationToken cancellationToken)
        {            
            var carts = await _repository.ListAsync(request.Page, request.Size, cancellationToken);
            
            var items = _mapper.Map<List<CartsListItem>>(carts);
            
            return new GetCartsListResult
            {
                Page = request.Page,
                Size = request.Size,
                Items = items
            };
        }
    }
}

