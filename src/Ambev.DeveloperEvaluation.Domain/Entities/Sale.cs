namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale, containing header information (ID, number, date, customer, branch)
    /// and an owned collection of <see cref="SaleItem"/> instances.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Gets the unique identifier of this sale.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the sale number (a business‐specific invoice or reference code).
        /// </summary>
        public string SaleNumber { get; private set; }

        /// <summary>
        /// Gets the date when this sale occurred.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the customer identifier (value object) associated with this sale.
        /// </summary>
        public CustomerId CustomerId { get; private set; }

        /// <summary>
        /// Gets the branch identifier (value object) where this sale was made.
        /// </summary>
        public string Branch { get; private set; }
                
        private readonly List<SaleItem> _items;

        /// <summary>
        /// Gets the read‐only collection of <see cref="SaleItem"/> lines belonging to this sale.
        /// </summary>
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Gets a value indicating whether this sale has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Used by EF Core for materialization. Initializes an empty items list.
        /// </summary>
        protected Sale()
        {
            _items = new List<SaleItem>();
        }

        /// <summary>
        /// Constructs a new <see cref="Sale"/> with the specified ID, sale number, date, 
        /// customer, and branch. No items are added initially.
        /// </summary>
        /// <param name="id">The unique identifier for this sale. Must be a non‐empty GUID.</param>
        /// <param name="saleNumber">The sale number (cannot be null or whitespace).</param>
        /// <param name="date">The date/time of the sale (must be within an acceptable range).</param>
        /// <param name="customerId">The customer identifier value object (cannot be null).</param>
        /// <param name="branch">The branch name (plain text) value object (cannot be null).</param>
        /// <exception cref="DomainException">Thrown if ID is empty or saleNumber is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if customerId or branchId is null.</exception>
        public Sale(Guid id, string saleNumber, DateTime date, CustomerId customerId, string branch)
            : this()
        {
            if (id == Guid.Empty)
                throw new DomainException("Sale ID must be a valid GUID.");
            if (string.IsNullOrWhiteSpace(saleNumber))
                throw new DomainException("SaleNumber cannot be empty.");
            if (customerId == null)
                throw new ArgumentNullException(nameof(customerId));           

            Id = id;
            SaleNumber = saleNumber;
            Date = date;
            CustomerId = customerId;
            Branch = branch;
            IsCancelled = false;
        }

        /// <summary>
        /// Adds a new <see cref="SaleItem"/> line to this sale. Calculates discount by quantity,
        /// generates a new GUID for the item, and assigns this sale's ID to it.
        /// </summary>
        /// <param name="productId">The product identifier (must be a non‐empty GUID).</param>
        /// <param name="quantity">The quantity of items sold (must be at least 1).</param>
        /// <param name="unitPrice">The unit price of the product (must be greater than zero).</param>
        /// <exception cref="DomainException">
        /// Thrown if the sale is cancelled, productId is empty, quantity &lt; 1, or unitPrice ≤ 0.
        /// </exception>
        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (IsCancelled)
                throw new DomainException("Cannot add items to a cancelled sale.");
            if (productId == Guid.Empty)
                throw new DomainException("ProductId must be a valid GUID.");
            if (quantity < 1)
                throw new DomainException("Quantity must be at least 1.");
            if (unitPrice <= 0)
                throw new DomainException("UnitPrice must be greater than zero.");

            var discountPercentage = CalculateDiscount(quantity);

            var item = new SaleItem(
                Guid.NewGuid(),
                productId,
                quantity,
                unitPrice,
                discountPercentage
            );

            item.SetSaleId(this.Id);

            _items.Add(item);
        }

        /// <summary>
        /// Removes all current <see cref="SaleItem"/> lines and replaces them with the provided collection.
        /// Each new item must not be null and will have its <c>SaleId</c> set to this sale's ID.
        /// </summary>
        /// <param name="newItems">The new collection of <see cref="SaleItem"/> instances to associate.</param>
        /// <exception cref="DomainException">
        /// Thrown if sale is cancelled or newItems is null or contains a null element.
        /// </exception>
        public void ReplaceItems(IEnumerable<SaleItem> newItems)
        {
            if (IsCancelled)
                throw new DomainException("Cannot replace items on a cancelled sale.");
            if (newItems == null)
                throw new DomainException("Item list cannot be null.");

            _items.Clear();

            foreach (var item in newItems)
            {
                if (item == null)
                    throw new DomainException("SaleItem cannot be null.");

                item.SetSaleId(this.Id);
                _items.Add(item);
            }
        }

        /// <summary>
        /// Cancels this sale and marks all associated <see cref="SaleItem"/> lines as cancelled.
        /// </summary>
        /// <exception cref="DomainException">Thrown if the sale is already cancelled.</exception>
        public void CancelSale()
        {
            if (IsCancelled)
                throw new DomainException("Sale is already cancelled.");

            IsCancelled = true;

            foreach (var item in _items)
                item.CancelItem();
        }

        /// <summary>
        /// Calculates the total amount for this sale by summing each item's <c>Total</c> (unit price × quantity × (1 − discount)).
        /// </summary>
        /// <returns>The sum of all <see cref="SaleItem.Total"/> values in this sale.</returns>
        public decimal TotalAmount()
        {
            return _items.Sum(i => i.Total);
        }

        /// <summary>
        /// Updates the sale number (invoice/reference code). Cannot be whitespace or null.
        /// </summary>
        /// <param name="newSaleNumber">The new sale number to assign.</param>
        /// <exception cref="DomainException">Thrown if newSaleNumber is null or whitespace.</exception>
        public void UpdateSaleNumber(string newSaleNumber)
        {
            if (string.IsNullOrWhiteSpace(newSaleNumber))
                throw new DomainException("SaleNumber cannot be empty.");

            SaleNumber = newSaleNumber;
        }

        /// <summary>
        /// Updates the date of the sale. New date cannot be more than 5 minutes in the future.
        /// </summary>
        /// <param name="newDate">The new date/time to set.</param>
        /// <exception cref="DomainException">Thrown if newDate is too far in the future.</exception>
        public void UpdateDate(DateTime newDate)
        {
            if (newDate > DateTime.UtcNow.AddMinutes(5))
                throw new DomainException("Sale date cannot be in the far future.");

            Date = newDate;
        }

        /// <summary>
        /// Updates the associated customer identifier for this sale.
        /// </summary>
        /// <param name="newCustomerId">The new customer identifier value object (cannot be null).</param>        
        /// <exception cref="DomainException">Thrown if either newCustomerId is null.</exception>
        public void UpdateCustomer(CustomerId newCustomerId/*, BranchId newBranchId*/)
        {
            if (newCustomerId == null)
                throw new DomainException("CustomerId cannot be null.");         

            CustomerId = newCustomerId;            
        }

        /// <summary>
        /// Updates the associated branch for this sale.
        /// </summary>       
        /// <param name="newBranch">The new branch in string format.</param>        
        public void UpdateBranch(string newBranch)
        {
            Branch = newBranch;
        }

        /// <summary>
        /// Determines the discount percentage based on quantity tier:
        /// <list type="bullet">
        ///   <item>No discount if quantity &lt; 4.</item>
        ///   <item>10% discount if quantity ≥ 4 and &lt; 10.</item>
        ///   <item>20% discount if quantity ≥ 10 and ≤ 20.</item>
        ///   <item>Throws <see cref="DomainException"/> if quantity &gt; 20.</item>
        /// </list>
        /// </summary>
        /// <param name="quantity">The quantity of items for which to calculate discount.</param>
        /// <returns>A decimal representing the discount percentage (0m, 0.10m, or 0.20m).</returns>
        /// <exception cref="DomainException">Thrown if quantity &gt; 20.</exception>
        public decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0m;
            if (quantity >= 4 && quantity < 10)
                return 0.10m;
            if (quantity >= 10 && quantity <= 20)
                return 0.20m;

            throw new DomainException("Cannot sell more than 20 identical items.");
        }
    }
}
