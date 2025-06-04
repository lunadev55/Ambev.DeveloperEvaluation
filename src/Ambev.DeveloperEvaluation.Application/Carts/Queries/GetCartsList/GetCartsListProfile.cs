using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartsList
{
    /// <summary>
    /// AutoMapper profile for mapping <see cref="Cart"/> to <see cref="CartsListItem"/> DTOs.
    /// </summary>
    public class GetCartsListProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartsListProfile"/> class,
        /// configuring the mapping from <see cref="Cart"/> to <see cref="CartsListItem"/>.
        /// </summary>
        public GetCartsListProfile()
        {
            CreateMap<Cart, CartsListItem>()
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.TotalAmount()));
        }
    }
}
