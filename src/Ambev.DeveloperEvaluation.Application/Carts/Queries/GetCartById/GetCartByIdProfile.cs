using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById
{
    /// <summary>
    /// AutoMapper profile for mapping <see cref="Cart"/> and its items to the query result DTOs.
    /// </summary>
    public class GetCartByIdProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartByIdProfile"/> class,
        /// configuring mappings from <see cref="Cart"/> to <see cref="GetCartByIdResult"/>
        /// and from <see cref="CartItem"/> to <see cref="CartItemResult"/>.
        /// </summary>
        public GetCartByIdProfile()
        {
            // Map Cart to GetCartByIdResult, computing TotalAmount via the domain method
            CreateMap<Cart, GetCartByIdResult>()
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.TotalAmount()));

            // Map each CartItem to CartItemResult, projecting the Total property
            CreateMap<CartItem, CartItemResult>()
                .ForMember(
                    dest => dest.Total,
                    opt => opt.MapFrom(src => src.Total));
        }
    }
}

