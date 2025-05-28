using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSalesList
{
    public class GetSalesListProfile : Profile
    {
        public GetSalesListProfile()
        {
            CreateMap<Sale, SalesListItem>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount()));
        }
    }
}
