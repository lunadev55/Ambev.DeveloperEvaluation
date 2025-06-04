namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Represents the outcome of an <see cref="UpdateCartCommand"/>, indicating whether the update succeeded.
    /// </summary>
    public class UpdateCartResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the cart update operation was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
