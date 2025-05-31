using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for sales operations
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Manager, Admin")]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>        
        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET /api/sales?_page=1&_size=10
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10)
        {
            var query = new GetSalesListQuery { Page = page, Size = size };
            var result = await _mediator.Send(query);

            // Wrap in your paginated response if you have PaginatedList<T>.
            // Here we return the raw result in a standard ApiResponseWithData wrapper.
            return Ok(result);
        }

        /// <summary>
        /// GET /api/sales/{id}
        /// </summary>
        //[HttpGet("{id}")]
        [HttpGet("{id}", Name = nameof(GetSaleById))]

        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var query = new GetSaleByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/sales
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(nameof(GetSaleById), new { id = result.Id }, result);
        }

        /// <summary>
        /// PUT /api/sales/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch between route and payload.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/sales/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var command = new CancelSaleCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
