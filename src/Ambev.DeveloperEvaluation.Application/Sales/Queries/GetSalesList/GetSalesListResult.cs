namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    public class GetSalesListResult
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public List<SalesListItem> Items { get; set; } = new List<SalesListItem>();
    }

    public class SalesListItem
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}
