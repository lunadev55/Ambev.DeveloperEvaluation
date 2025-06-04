using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList
{
    public class GetProductsListQuery : IRequest<GetProductsListResult>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string OrderBy { get; set; }
    }
}
