using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    /// <summary>
    /// AutoMapper profile for mapping <see cref="Sale"/> and its items to the query result DTOs.
    /// </summary>
    public class GetSaleByIdProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleByIdProfile"/> class,
        /// configuring mappings from <see cref="Sale"/> to <see cref="GetSaleByIdResult"/>
        /// and from <see cref="SaleItem"/> to <see cref="SaleItemResult"/>.
        /// </summary>
        public GetSaleByIdProfile()
        {
            // Map Sale to GetSaleByIdResult, computing TotalAmount via the domain method
            CreateMap<Sale, GetSaleByIdResult>()
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.TotalAmount()));

            // Map each SaleItem to SaleItemResult, projecting the Total property
            CreateMap<SaleItem, SaleItemResult>()
                .ForMember(
                    dest => dest.Total,
                    opt => opt.MapFrom(src => src.Total));
        }
    }
}

