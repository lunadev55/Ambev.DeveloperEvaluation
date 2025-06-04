using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCartById
{
    /// <summary>
    /// AutoMapper profile for mapping domain <see cref="Cart"/> entities (including value objects)
    /// to <see cref="GetCartByIdResult"/> and <see cref="CartItemResult"/> DTOs.
    /// </summary>
    public class GetCartByIdProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCartByIdProfile"/> class,
        /// configuring mappings from <see cref="CustomerId"/> and <see cref="BranchId"/> value objects
        /// to <see cref="Guid"/>, and from <see cref="Cart"/> and <see cref="CartItem"/> entities
        /// to their respective result DTOs.
        /// </summary>
        public GetCartByIdProfile()
        {            
            CreateMap<CustomerId, Guid>()
                .ConvertUsing(src => src.Value);
            CreateMap<BranchId, Guid>()
                .ConvertUsing(src => src.Value);
                        
            CreateMap<Cart, GetCartByIdResult>()
                .ForMember(
                    dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(
                    dest => dest.Branch,
                    opt => opt.MapFrom(src => src.Branch))
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.TotalAmount()))
                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));
                        
            CreateMap<CartItem, CartItemResult>()
                .ForMember(
                    dest => dest.Total,
                    opt => opt.MapFrom(src => src.Total));
        }
    }
}


