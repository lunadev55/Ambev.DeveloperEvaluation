using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<GetProductByIdResult>
    {
        public Guid Id { get; set; }
    }
}
