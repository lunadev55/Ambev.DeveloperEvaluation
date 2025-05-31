using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            // map command -> domain entity
            CreateMap<CreateSaleCommand, Sale>()
                .ConstructUsing(cmd => new Sale(
                    Guid.NewGuid(),
                    cmd.SaleNumber,
                    cmd.Date,
                    new CustomerId(cmd.CustomerId),
                    new BranchId(cmd.BranchId)));

            // map item DTO -> domain item
            CreateMap<CreateSaleItemDto, SaleItem>()
                .ConstructUsing(dto => new SaleItem(
                    Guid.NewGuid(),
                    dto.ProductId,
                    dto.Quantity,
                    dto.UnitPrice,
                    0m /* domain calculates discount internally */));
        }
    }
}
