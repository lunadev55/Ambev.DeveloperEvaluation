using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// AutoMapper profile for mapping between CreateCart DTOs and domain entities.
    /// </summary>
    public class CreateCartProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCartProfile"/> class,
        /// configuring mappings from <see cref="CreateCartCommand"/> to <see cref="Cart"/>,
        /// and from <see cref="CreateCartItemDto"/> to <see cref="CartItem"/>.
        /// </summary>
        public CreateCartProfile()
        {            
            CreateMap<CreateCartCommand, Cart>()
                .ConstructUsing(cmd => new Cart(
                    Guid.NewGuid(),
                    cmd.CartNumber, 
                    cmd.Date,
                    new CustomerId(cmd.CustomerId),
                    cmd.Branch));
                        
            CreateMap<CreateCartItemDto, CartItem>()
                .ConstructUsing(dto => new CartItem(
                    Guid.NewGuid(),
                    dto.ProductId,
                    dto.Quantity,
                    dto.UnitPrice,
                    0m /* discount will be calculated by domain logic */));
        }
    }
}

