namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public bool IsCancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItemResult> Items { get; set; } = new List<SaleItemResult>();
    }

    public class SaleItemResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsCancelled { get; set; }
        public decimal Total { get; set; }
    }
}
