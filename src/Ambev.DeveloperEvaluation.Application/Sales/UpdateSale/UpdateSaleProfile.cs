using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="UpdateSaleCommand"/>/ 
    /// <see cref="UpdateSaleItemDto"/> DTOs and domain <see cref="Sale"/>/ 
    /// <see cref="SaleItem"/> entities.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleProfile"/> class,
        /// configuring mappings for updating existing sales and their items.
        /// </summary>
        public UpdateSaleProfile()
        {            
            CreateMap<UpdateSaleCommand, Sale>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id));
                        
            CreateMap<UpdateSaleItemDto, SaleItem>()
                .ConstructUsing(dto => new SaleItem(
                    Guid.NewGuid(),
                    dto.ProductId,
                    dto.Quantity,
                    dto.UnitPrice,
                    0m /* discount is applied in Sale.CalculateDiscount */));
        }
    }
}
