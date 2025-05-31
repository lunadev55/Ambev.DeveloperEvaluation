using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductById
{
    public class GetProductByIdProfile : Profile
    {
        public GetProductByIdProfile()
        {
            CreateMap<Product, GetProductByIdResult>()
                .ForMember(dest => dest.RatingRate, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Rating.Count));
        }
    }
}
