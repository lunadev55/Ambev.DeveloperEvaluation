using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdQuery : IRequest<GetSaleByIdResult>
    {
        public Guid Id { get; set; }
    }
}
