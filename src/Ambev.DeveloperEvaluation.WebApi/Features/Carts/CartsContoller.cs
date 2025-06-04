using Ambev.DeveloperEvaluation.Application.Carts.CancelCart;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById;
using Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartsList;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts
{
    /// <summary>
    /// Controller for carts operations
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Manager, Admin")]
    [Route("api/[controller]")]
    public class CartsController : BaseController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of CartsController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>        
        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET /api/carts?_page=1&_size=10
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10)
        {
            var query = new GetCartsListQuery { Page = page, Size = size };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// GET /api/carts/{id}
        /// </summary>
        //[HttpGet("{id}")]
        [HttpGet("{id}", Name = nameof(GetCartById))]

        public async Task<IActionResult> GetCartById(Guid id)
        {
            var query = new GetCartByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/carts
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(nameof(GetCartById), new { id = result.Id }, result);
        }

        /// <summary>
        /// PUT /api/carts/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCartCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch between route and payload.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/carts/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var command = new CancelCartCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
