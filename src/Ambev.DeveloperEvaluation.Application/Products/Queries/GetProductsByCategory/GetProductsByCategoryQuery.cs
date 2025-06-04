using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQuery : IRequest<GetProductsByCategoryResult>
    {
        public string Category { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string OrderBy { get; set; }
    }
}
