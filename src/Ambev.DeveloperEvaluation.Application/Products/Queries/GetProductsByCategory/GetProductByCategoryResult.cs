using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryResult
    {
        public IEnumerable<ProductListItem> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
