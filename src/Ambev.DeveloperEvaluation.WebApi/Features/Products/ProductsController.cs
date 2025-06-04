using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductById;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// GET /api/products?_page=1&_size=10&_order="price desc,title asc"
        /// </summary>
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

        /// <summary>
        /// GET /api/products/{id}
        /// </summary>
        [HttpGet("{id}", Name = nameof(GetProductById))]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/products/categories
        /// </summary>
        [HttpGet("categories", Name = nameof(GetCategories))]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/products/category/{category}?_page=1&_size=10&_order=...
        /// </summary>
        [HttpGet("category/{category}", Name = nameof(GetByCategory))]
        public async Task<IActionResult> GetByCategory(
            string category,
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10,
            [FromQuery(Name = "_order")] string order = null)
        {
            var query = new GetProductsByCategoryQuery { Category = category, Page = page, Size = size, OrderBy = order };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/products
        /// </summary>  
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(nameof(GetProductById), new { id = result.Id }, result);
        }

        /// <summary>
        /// PUT /api/products/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch between route and payload.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/products/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
