//using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById;
//using Ambev.DeveloperEvaluation.Domain.Entities;
//using AutoMapper;

//namespace Ambev.DeveloperEvaluation.WebApi.Mappings
//{
//    public class GetSaleByIdProfile : Profile
//    {
//        public GetSaleByIdProfile()
//        {
//            CreateMap<Sale, GetSaleByIdResult>()
//                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Value))
//                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId.Value))
//                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount()))
//                // this line wires up the items collection
//                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

//            CreateMap<SaleItem, SaleItemResult>()
//                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
//        }
//    }
//}

// File: src/Ambev.DeveloperEvaluation.Application/Sales/Queries/GetSaleById/GetSaleByIdProfile.cs
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdProfile : Profile
    {
        public GetSaleByIdProfile()
        {
            // Value object mappings
            CreateMap<CustomerId, Guid>().ConvertUsing(src => src.Value);
            CreateMap<BranchId, Guid>().ConvertUsing(src => src.Value);

            // Entity to DTO mappings
            CreateMap<Sale, GetSaleByIdResult>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, SaleItemResult>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
        }
    }
}

