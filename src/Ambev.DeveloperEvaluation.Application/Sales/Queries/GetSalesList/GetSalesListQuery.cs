using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    /// <summary>
    /// Represents a request for a paginated list of sales.
    /// </summary>
    public class GetSalesListQuery : IRequest<GetSalesListResult>
    {
        /// <summary>
        /// Gets or sets the page number to retrieve (1-based). Defaults to 1.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page. Defaults to 10.
        /// </summary>
        public int Size { get; set; } = 10;
    }
}


