namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart
{
    /// <summary>
    /// Represents the outcome of a <see cref="CancelCartCommand"/>, indicating whether the cancellation succeeded.
    /// </summary>
    public class CancelCartResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the cart cancellation was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
