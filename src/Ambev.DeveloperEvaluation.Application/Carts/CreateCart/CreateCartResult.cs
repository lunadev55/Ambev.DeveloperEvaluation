namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Represents the outcome of a <see cref="CreateCartCommand"/>, 
    /// containing the unique identifier of the newly created cart.
    /// </summary>
    public class CreateCartResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart that was created.
        /// </summary>
        public Guid Id { get; set; }
    }
}

