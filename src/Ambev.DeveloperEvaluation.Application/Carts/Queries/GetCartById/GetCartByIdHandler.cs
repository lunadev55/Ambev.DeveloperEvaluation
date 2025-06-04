using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById
{
    /// <summary>
    /// Handles requests to retrieve a cart by its unique identifier.
    /// </summary>
    public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, GetCartByIdResult>
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartByIdHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository used to access <see cref="Domain.Entities.Cart"/> aggregates.
        /// </param>
        /// <param name="mapper">
        /// The AutoMapper instance used to project domain entities into result DTOs.
        /// </param>
        public GetCartByIdHandler(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes a <see cref="GetCartByIdQuery"/> and returns the corresponding
        /// <see cref="GetCartByIdResult"/> if found; otherwise throws a <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="request">
        /// The query containing the ID of the cart to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A <see cref="GetCartByIdResult"/> mapping the requested cart's data.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no cart with the specified ID exists in the repository.
        /// </exception>
        public async Task<GetCartByIdResult> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {            
            var cart = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID '{request.Id}' not found.");          

            return _mapper.Map<GetCartByIdResult>(cart);
        }
    }
}

