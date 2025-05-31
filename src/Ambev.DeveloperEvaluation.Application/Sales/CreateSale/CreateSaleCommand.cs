using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();
    }

    public class CreateSaleItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
