using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    public class GetSalesListQuery : IRequest<GetSalesListResult>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
