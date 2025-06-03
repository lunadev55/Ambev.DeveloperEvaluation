using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    /// <summary>
    /// AutoMapper profile for mapping <see cref="Sale"/> to <see cref="SalesListItem"/> DTOs.
    /// </summary>
    public class GetSalesListProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSalesListProfile"/> class,
        /// configuring the mapping from <see cref="Sale"/> to <see cref="SalesListItem"/>.
        /// </summary>
        public GetSalesListProfile()
        {
            CreateMap<Sale, SalesListItem>()
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.TotalAmount()));
        }
    }
}
