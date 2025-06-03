using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handles the <see cref="UpdateSaleCommand"/> by replacing all existing items for a sale
    /// and updating the sale header (number, date, customer, branch).
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The <see cref="ISaleRepository"/> used to load, delete, and save sale and sale-item data.
        /// </param>
        public UpdateSaleHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Processes the update request by:
        /// 1. Loading the existing <see cref="Sale"/> aggregate (including its items).
        /// 2. Updating header fields (sale number, date, customer, branch).
        /// 3. Deleting all currently stored <see cref="SaleItem"/> rows for that sale.
        /// 4. Inserting new <see cref="SaleItem"/> rows based on the incoming DTOs.
        /// 5. Marking the sale header as modified and persisting any changes.
        /// </summary>
        /// <param name="request">
        /// The <see cref="UpdateSaleCommand"/> containing the sale ID, new header values, and item DTOs.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe for cancellation requests.
        /// </param>
        /// <returns>
        /// An <see cref="UpdateSaleResult"/> indicating success or failure.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no sale with the specified ID exists.
        /// </exception>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {            
            var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID '{request.Id}' not found.");
            
            sale.UpdateSaleNumber(request.SaleNumber);
            sale.UpdateDate(request.Date);
            sale.UpdateCustomer(new CustomerId(request.CustomerId), new BranchId(request.BranchId));
            
            var newItems = request.Items
                .Select(dto =>
                {
                    var discountPct = sale.CalculateDiscount(dto.Quantity);
                    return new SaleItem(
                        Guid.NewGuid(),
                        dto.ProductId,
                        dto.Quantity,
                        dto.UnitPrice,
                        discountPct
                    );
                })
                .ToList();
            
            await _repository.DeleteAllItemsBySaleIdAsync(sale.Id, cancellationToken);
            
            await _repository.AddItemsAsync(newItems, sale.Id, cancellationToken);
            
            _repository.Update(sale);
            
            await _repository.SaveChangesAsync(cancellationToken);

            return new UpdateSaleResult { Success = true };
        }
    }
}
