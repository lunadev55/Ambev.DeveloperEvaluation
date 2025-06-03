namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Represents the outcome of an <see cref="UpdateSaleCommand"/>, indicating whether the update succeeded.
    /// </summary>
    public class UpdateSaleResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the sale update operation was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
