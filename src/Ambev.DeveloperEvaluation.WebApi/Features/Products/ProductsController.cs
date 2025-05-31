// File: src/Ambev.DeveloperEvaluation.WebApi/Controllers/ProductsController.cs
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductById;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager,Admin")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/products?_page=1&_size=10&_order="price desc,title asc"
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10,
            [FromQuery(Name = "_order")] string order = null)
        {
            var query = new GetProductsListQuery { Page = page, Size = size, OrderBy = order };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // GET /api/products/{id}
        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST /api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT /api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch between route and payload.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
