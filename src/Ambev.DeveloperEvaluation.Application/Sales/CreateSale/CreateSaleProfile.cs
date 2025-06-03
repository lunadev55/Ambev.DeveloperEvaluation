using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile for mapping between CreateSale DTOs and domain entities.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleProfile"/> class,
        /// configuring mappings from <see cref="CreateSaleCommand"/> to <see cref="Sale"/>,
        /// and from <see cref="CreateSaleItemDto"/> to <see cref="SaleItem"/>.
        /// </summary>
        public CreateSaleProfile()
        {            
            CreateMap<CreateSaleCommand, Sale>()
                .ConstructUsing(cmd => new Sale(
                    Guid.NewGuid(),
                    cmd.SaleNumber,
                    cmd.Date,
                    new CustomerId(cmd.CustomerId),
                    new BranchId(cmd.BranchId)));
                        
            CreateMap<CreateSaleItemDto, SaleItem>()
                .ConstructUsing(dto => new SaleItem(
                    Guid.NewGuid(),
                    dto.ProductId,
                    dto.Quantity,
                    dto.UnitPrice,
                    0m /* discount will be calculated by domain logic */));
        }
    }
}

