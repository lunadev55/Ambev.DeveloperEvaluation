using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    /// <summary>
    /// Handles requests to retrieve a sale by its unique identifier.
    /// </summary>
    public class GetSaleByIdHandler : IRequestHandler<GetSaleByIdQuery, GetSaleByIdResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleByIdHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to access <see cref="Domain.Entities.Sale"/> aggregates.
        /// </param>
        /// <param name="mapper">
        /// The AutoMapper instance used to project domain entities into result DTOs.
        /// </param>
        public GetSaleByIdHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes a <see cref="GetSaleByIdQuery"/> and returns the corresponding
        /// <see cref="GetSaleByIdResult"/> if found; otherwise throws a <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="request">
        /// The query containing the ID of the sale to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A <see cref="GetSaleByIdResult"/> mapping the requested sale's data.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no sale with the specified ID exists in the repository.
        /// </exception>
        public async Task<GetSaleByIdResult> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {            
            var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID '{request.Id}' not found.");          

            return _mapper.Map<GetSaleByIdResult>(sale);
        }
    }
}

