using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            // Map command to existing Sale entity (for validation or patching scenarios)
            CreateMap<UpdateSaleCommand, Sale>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // Map DTO to domain item (discount handled by domain)
            CreateMap<SaleItemDto, SaleItem>()
                .ConstructUsing(dto => new SaleItem(
                    Guid.NewGuid(),
                    dto.ProductId,
                    dto.Quantity,
                    dto.UnitPrice,
                    0m));
        }
    }
}
