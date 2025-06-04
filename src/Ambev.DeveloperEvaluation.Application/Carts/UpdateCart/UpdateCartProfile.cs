using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="UpdateCartCommand"/>/ 
    /// <see cref="UpdateCartItemDto"/> DTOs and domain <see cref="Cart"/>/ 
    /// <see cref="CartItem"/> entities.
    /// </summary>
    public class UpdateCartProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCartProfile"/> class,
        /// configuring mappings for updating existing carts and their items.
        /// </summary>
        public UpdateCartProfile()
        {            
            CreateMap<UpdateCartCommand, Cart>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id));
                        
            CreateMap<UpdateCartItemDto, CartItem>()
                .ConstructUsing(dto => new CartItem(
                    Guid.NewGuid(),
                    dto.ProductId,
                    dto.Quantity,
                    dto.UnitPrice,
                    0m /* discount is applied in Cart.CalculateDiscount */));
        }
    }
}
