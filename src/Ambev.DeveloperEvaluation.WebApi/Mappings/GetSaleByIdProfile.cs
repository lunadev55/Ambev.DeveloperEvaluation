using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    /// <summary>
    /// AutoMapper profile for mapping domain <see cref="Sale"/> entities (including value objects)
    /// to <see cref="GetSaleByIdResult"/> and <see cref="SaleItemResult"/> DTOs.
    /// </summary>
    public class GetSaleByIdProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleByIdProfile"/> class,
        /// configuring mappings from <see cref="CustomerId"/> and <see cref="BranchId"/> value objects
        /// to <see cref="Guid"/>, and from <see cref="Sale"/> and <see cref="SaleItem"/> entities
        /// to their respective result DTOs.
        /// </summary>
        public GetSaleByIdProfile()
        {            
            CreateMap<CustomerId, Guid>()
                .ConvertUsing(src => src.Value);
            CreateMap<BranchId, Guid>()
                .ConvertUsing(src => src.Value);
                        
            CreateMap<Sale, GetSaleByIdResult>()
                .ForMember(
                    dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(
                    dest => dest.BranchId,
                    opt => opt.MapFrom(src => src.BranchId))
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.TotalAmount()))
                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));
                        
            CreateMap<SaleItem, SaleItemResult>()
                .ForMember(
                    dest => dest.Total,
                    opt => opt.MapFrom(src => src.Total));
        }
    }
}


