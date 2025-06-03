namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents the outcome of a <see cref="CreateSaleCommand"/>, 
    /// containing the unique identifier of the newly created sale.
    /// </summary>
    public class CreateSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale that was created.
        /// </summary>
        public Guid Id { get; set; }
    }
}

