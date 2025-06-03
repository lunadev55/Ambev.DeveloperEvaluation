namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Represents the outcome of a <see cref="CancelSaleCommand"/>, indicating whether the cancellation succeeded.
    /// </summary>
    public class CancelSaleResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the sale cancellation was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
