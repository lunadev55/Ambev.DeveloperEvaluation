namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList
{
    public class GetProductsListResult
    {
        public IEnumerable<ProductListItem> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }

    public class ProductListItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
